using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Users;
using VinPocket.Api.Models;
using VinPocket.Api.Utilities;
using BC = BCrypt.Net.BCrypt;

namespace VinPocket.Api.Controllers;

[ApiController]
[Route("api/auth")]

public sealed class UsersController(
    AppDbContext context,
    TokenProvider tokenProvider) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [EndpointSummary("Register a new user")]
    [EndpointDescription("Creates a new user account with the provided registration details and returns access tokens for immediate authentication.")]
    [ProducesResponseType<UserRegistrationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserRegistrationResponse>> Register(
        [FromBody] UserRegistrationRequestDto request,
        IValidator<UserRegistrationRequestDto> validator,
        CancellationToken cancellationToken)
    {

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(errors);
        }

        bool isEmailTaken = await context.Users.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken) is not null;

        if (isEmailTaken)
        {
            return Problem(detail: $"Email '{request.Email}' is already taken",
                statusCode: StatusCodes.Status409Conflict);
        }

        var newUser = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BC.EnhancedHashPassword(request.Password)
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(new UserRegistrationResponse
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [EndpointSummary("Authenticate user")]
    [EndpointDescription("Authenticates a user with their email and password, returning access and refresh tokens upon successful login.")]
    [ProducesResponseType<UserLoginResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserLoginResponseDto>> Login(
        [FromBody] UserLoginRequestDto request,
        IValidator<UserLoginRequestDto> validator,
        CancellationToken cancellationToken)
    {

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(errors);
        }

        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
            return Unauthorized();

        if (!BC.EnhancedVerify(request.Password, user.PasswordHash))
            return Unauthorized();

        var access = tokenProvider.Create(user);

        var (refreshToken, expiresAt) = tokenProvider.GenerateRefreshToken();

        var rt = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = expiresAt
        };

        context.RefreshTokens.Add(rt);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(new UserLoginResponseDto { AccessToken = access, RefreshToken = refreshToken });
    }

    [Authorize]
    [HttpDelete("logout")]
    [EndpointSummary("Revoke user's token")]
    [EndpointDescription("Logout user and revoke all refresh tokens in the database.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<UserLoginResponseDto>> Logout()
    {
        Guid userId = User.GetUserId();

        await context.RefreshTokens
           .Where(r => r.UserId == userId)
           .ExecuteDeleteAsync();

        return NoContent();
    }
}

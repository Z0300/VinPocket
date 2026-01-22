using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinPocket.Api.Common.Auth;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Auth;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Extensions;
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
    [ProducesResponseType<AccessTokensDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AccessTokensDto>> Register(
        RegisterUserDto registerUserDto,
        IValidator<RegisterUserDto> validator,
        CancellationToken cancellationToken)
    {

        await validator.ValidateAndThrowAsync(registerUserDto, cancellationToken);

        bool isEmailTaken = await context.Users
            .SingleOrDefaultAsync(x => x.Email == registerUserDto.Email, cancellationToken) is not null;

        if (isEmailTaken)
        {
            return Problem(detail: $"Email '{registerUserDto.Email}' is already taken",
                statusCode: StatusCodes.Status409Conflict);
        }

        User user = registerUserDto.ToEntity();

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        AccessTokensDto accessTokens = tokenProvider.Create(user);

        var rt = new RefreshToken
        {
            Id = $"r_{Guid.CreateVersion7()}",
            Token = accessTokens.RefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };

        context.RefreshTokens.Add(rt);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(accessTokens);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [EndpointSummary("Authenticate user")]
    [EndpointDescription("Authenticates a user with their email and password, returning access and refresh tokens upon successful login.")]
    [ProducesResponseType<AccessTokensDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AccessTokensDto>> Login(
        LoginUserDto loginUserDto,
        IValidator<LoginUserDto> validator,
        CancellationToken cancellationToken)
    {

        await validator.ValidateAndThrowAsync(loginUserDto, cancellationToken);

        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == loginUserDto.Email, cancellationToken);

        if (user is null)
            return Unauthorized();

        if (!BC.EnhancedVerify(loginUserDto.Password, user.PasswordHash))
            return Unauthorized();

        AccessTokensDto accessTokens = tokenProvider.Create(user);

        var rt = new RefreshToken
        {
            Id = $"r_{Guid.CreateVersion7()}",
            Token = accessTokens.RefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };

        context.RefreshTokens.Add(rt);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(accessTokens);
    }

    [Authorize]
    [HttpDelete("logout")]
    [EndpointSummary("Revoke user's token")]
    [EndpointDescription("Logout user and revoke all refresh tokens in the database.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<AccessTokensDto>> Logout()
    {
        string? userId = User.GetUserId();

        await context.RefreshTokens
           .Where(r => r.UserId == userId)
           .ExecuteDeleteAsync();

        return NoContent();
    }
}

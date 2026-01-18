using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinPocket.Api.Contracts.Users;
using VinPocket.Api.Data;
using VinPocket.Api.Models;
using VinPocket.Api.Utilities;
using BC = BCrypt.Net.BCrypt;

namespace VinPocket.Api.Controllers;

[ApiController]
[Route("api/users")]

public class UsersController(
    AppDbContext context,
    TokenProvider tokenProvider) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserRegistrationResponse>> Register(
        [FromBody] UserRegistrationRequest request, 
        IValidator<UserRegistrationRequest> validator,
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
    public async Task<ActionResult<UserLoginResponse>> Login(
        [FromBody] UserLoginRequest request,
        IValidator<UserLoginRequest> validator,
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

        return Ok(new UserLoginResponse { AccessToken = access, RefreshToken = refreshToken });
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<ActionResult<UserLoginResponse>> Logout()
    {
        Guid userId = User.GetUserId();

        await context.RefreshTokens
           .Where(r => r.UserId == userId)
           .ExecuteDeleteAsync();

        return NoContent();
    }
}

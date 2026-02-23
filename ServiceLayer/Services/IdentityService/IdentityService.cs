using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Models.ConfigModels;
using Common.Models.Identity;
using Identity.Persistence;
using IdentityCore.Enums;
using IdentityCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ServiceLayer.Services.IdentityService;

public class IdentityService : IIdentityService
{
    private readonly JwtSettingModel _jwtSetting;
    private readonly IdentityContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public IdentityService(IOptions<JwtSettingModel> jwtSetting, IdentityContext context,
        PasswordHasher<User> passwordHasher)
    {
        _jwtSetting = jwtSetting.Value;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> CreateJwtTokenAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
            .ConfigureAwait(false) ?? throw new NotFoundException<User>("User not found");

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey));

        var token = new JwtSecurityToken(
            audience: _jwtSetting.Audience,
            issuer: _jwtSetting.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(_jwtSetting.ValidHours)),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> IsCredentialsValidAsync(AuthModel authModel, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authModel.Email, cancellationToken)
            .ConfigureAwait(false);
        return user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, authModel.Password) ==
            PasswordVerificationResult.Success;
    }

    public async Task<Guid> RegisterUserAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
    {
        var emailAlreadyUsed = await _context.Users.AnyAsync(u => u.Email == userDto.Email, cancellationToken)
            .ConfigureAwait(false);
        if (emailAlreadyUsed)
        {
            throw new PropertyValidationException("Email already used");
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Email = userDto.Email,
            IsDeleted = false,
            Name = userDto.Name,
            Role = Role.User
        };
        user.Password = _passwordHasher.HashPassword(user, userDto.Password);

        await _context.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0
            ? user.Id
            : throw new DatabaseSaveChangesException("Unable to register user");
    }

    public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken) => await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id, cancellationToken).ConfigureAwait(false);

    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken) => await _context.Users
        .ToListAsync(cancellationToken).ConfigureAwait(false);

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken).ConfigureAwait(false) ??
                   throw new NotFoundException<User>("User not found");
        user.IsDeleted = true;
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    public async Task<bool> UpdateEmailAsync(Guid id, string email, CancellationToken cancellationToken)
    {
        var emailAlreadyUsed =
            await _context.Users.AnyAsync(u => u.Email == email, cancellationToken).ConfigureAwait(false);
        if(emailAlreadyUsed)
            throw new PropertyValidationException("Email already used");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken).ConfigureAwait(false) ??
                   throw new NotFoundException<User>("User not found");
        user.Email = email;
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }
}
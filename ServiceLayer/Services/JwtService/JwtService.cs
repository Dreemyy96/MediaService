using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Models.ConfigModels;
using Common.Models.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ServiceLayer.Services.JwtService;

public class JwtService : IJwtService
{
    private readonly JwtSettingModel _jwtSetting;

    public JwtService(IOptions<JwtSettingModel> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
    }

    public string GenerateJwtToken(ClaimModel claimModel)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, claimModel.UserId.ToString()));
        claims.Add(new Claim(ClaimTypes.Role, claimModel.Role));
        claims.Add(new Claim(ClaimTypes.Email, claimModel.Email));
        
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
}
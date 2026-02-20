using Common.Models.Identity;

namespace ServiceLayer.Services.JwtService;

public interface IJwtService
{
    string GenerateJwtToken(ClaimModel claimModel);
}
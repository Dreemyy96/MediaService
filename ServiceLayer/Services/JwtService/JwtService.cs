using Common.Models.ConfigModels;
using Microsoft.Extensions.Options;

namespace ServiceLayer.Services.JwtService;

public class JwtService
{
    private readonly JwtSettingModel _jwtSetting;

    public JwtService(IOptions<JwtSettingModel> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
    }
    
}
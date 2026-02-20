namespace Common.Models.ConfigModels;

public class JwtSettingModel
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
    public int ValidHours { get; set; }
}
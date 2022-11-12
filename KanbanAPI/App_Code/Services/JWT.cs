using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthOptions
{
    public const string ISSUER = "https://localhost:5001"; // издатель токена
    public const string AUDIENCE = "https://localhost:5001"; // потребитель токена
    const string KEY = "P(%@#*VBPsdQ*$V%#(sdfX@ASD^%Bsdf#VT@#*$%P&C@#%*&@B@";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    public const int DurationMins = 120;
    public const int DurationDays = 30;
}
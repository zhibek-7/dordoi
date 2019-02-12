using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Localization.Authentication
{
    public class AuthenticationOptions
    {
        public const string ISSUER = "LocalizationApp"; // издатель токена
        public const string AUDIENCE = "http://localhost:52095/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 5; // время жизни токена - 3 минуты
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

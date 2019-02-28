using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Utilities;

namespace Localization.Authentication
{
    public class AuthenticationOptions
    {
        private Settings st = new Settings();
        //TODO НУжно переложить ключь в другое место
        private const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

        public string ISSUER { get; }// = "LocalizationApp"; // издатель токена
        public string AUDIENCE { get; }// = "http://localhost:52095/"; // потребитель токена
        public int LIFETIME { get; }// = 15; // время жизни токена - 60 минут

        public AuthenticationOptions()
        {
            ISSUER = st.GetString("AuthenticationOptions_ISSUER");
            AUDIENCE = st.GetString("AuthenticationOptions_AUDIENCE");
            LIFETIME = Int32.Parse(st.GetString("AuthenticationOptions_LIFETIME"));
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

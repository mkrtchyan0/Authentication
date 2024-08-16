using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Authentication.Api.JwtToken
{
    public class JwtToken
    {
        public JwtToken(IConfiguration config) { _config = config; }
        private IConfiguration _config;
        public string Create()
        {
            var key = _config["Authentication:Schemes:Bearer:SigningKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "weak key"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}

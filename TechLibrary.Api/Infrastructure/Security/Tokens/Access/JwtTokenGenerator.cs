using TechLibrary.Api.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TechLibrary.Api.Infrastructure.Security.Tokens.Access
{
    public class JwtTokenGenerator
    {

        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) 
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(60),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)   //HEADER + PAYLOAD + PT1 e PT2 Criptografadas (assim é composto o token)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(securityToken);
        }

        private static SymmetricSecurityKey SecurityKey()
        {
            var signingkey = "V72Cin552hbN9UA32rFxq28q7i1R0UTp"; //Nao pode ficar explicita no código fonte
            var symmetricKey = Encoding.UTF8.GetBytes(signingkey);

            return new SymmetricSecurityKey(symmetricKey);
        }

    }
}

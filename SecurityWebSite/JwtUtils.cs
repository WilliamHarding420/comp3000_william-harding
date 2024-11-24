using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecurityWebSite
{
    public class JwtUtils
    {

        public async static Task<string> AuthorizeUser(int userID)
        {

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Program.SECURITY_KEY));
            SigningCredentials signingCredential = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] tokenClaims =
            {
                new Claim("userID", userID.ToString())
            };

            JwtSecurityToken authToken = new(
                issuer: "security-website",
                audience: "user",
                claims: tokenClaims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signingCredential);

            return new JwtSecurityTokenHandler().WriteToken(authToken);

        }

    }
}

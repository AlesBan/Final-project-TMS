using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Playlist_for_party.Configuration;

namespace Playlist_for_party.Models.Authentication
{
    public class Authentication
    {
        public static string GenerateToken(JwtSettings jwtSettings, string userName, List<string> roles)
        {
            var jwtClaims = new List<Claim>
            {
                new (ClaimTypes.Name, userName),
            };

            roles.ForEach(role => jwtClaims.Add(new Claim(ClaimTypes.Role, role)));

            var singingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(3);

            var jwt = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: jwtClaims,
                expires: expires,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
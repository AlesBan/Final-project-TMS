using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApp_Data.Models;

namespace Playlist_for_party.Authentication
{
    public static class Authentication
    {
        private const int ExpiresDays = 3;
        public static string GenerateToken(IConfiguration configuration, User user, List<string> roles)
        {
            var jwt = GetJwtSecurityToken(configuration, user, roles);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static JwtSecurityToken GetJwtSecurityToken(IConfiguration configuration, User user, List<string> roles)
        {
            var jwtClaims = GetClaims(user, roles);
            var singingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWTSettings:SecretKey"]));
            var credentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(ExpiresDays);
            
            var jwt = new JwtSecurityToken(
                issuer: configuration["JWTSettings:Issuer"],
                audience: configuration["JWTSettings:Audience"],
                claims: jwtClaims,
                expires: expires,
                signingCredentials: credentials
            );
            
            return jwt;
        }

        private static IEnumerable<Claim> GetClaims( User user, List<string> roles)
        {
            var jwtClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            roles.ForEach(role => jwtClaims.Add(new Claim(ClaimTypes.Role, role)));

            return jwtClaims;
        }
    }
}
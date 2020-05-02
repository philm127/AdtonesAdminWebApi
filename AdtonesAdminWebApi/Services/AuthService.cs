using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AdtonesAdminWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _secret;
        private readonly string _expDate;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration config)
        {
            // _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            // _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
            _configuration = config;
        }

        public string GenerateSecurityToken(User userModel)
        {
            string jwtToken = string.Empty;

            // Read the secret key and the expiration from the configuration 
            var secretKey = Convert.FromBase64String(_configuration["JwtConfig:secret"]);
            var expiryTimeSpan = Convert.ToInt32(_configuration["JwtConfig:expirationInMinutes"]);

            // IdentityUser user = new IdentityUser(userModel.Email);

            var securityTokenDescription = new SecurityTokenDescriptor()
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("userId",userModel.UserId.ToString()),
                        new Claim("roleId",userModel.RoleId.ToString()),
                        new Claim("role",userModel.Role)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(expiryTimeSpan),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)

            };

            // Generate token using JwtSecurityTokenHandler.
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwToken = jwtHandler.CreateJwtSecurityToken(securityTokenDescription);
            jwtToken = jwtHandler.WriteToken(jwToken);

            return jwtToken;

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.Email, email)
            //    }),
            //    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);
        }
    }
}
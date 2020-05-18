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
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration config)
        {
            _configuration = config;
        }


        public string GenerateSecurityToken(User userModel)
        {
            // Read the secret key and the expiration from the configuration 
            var secretKey = Convert.FromBase64String(_configuration["JwtConfig:secret"]);
            var expiryTimeSpan = Convert.ToInt32(_configuration["JwtConfig:expirationInMinutes"]);

            // When picking up claim names are returned in JWT as camel case so hunting for values later
            // remember that UserId will be userId. Exception is Role as is a ClaimType,don't know why UserId or RoleId, maybe Role
            // normally used Roleid.
            var securityTokenDescription = new SecurityTokenDescriptor()
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("UserId",userModel.UserId.ToString()),
                        new Claim("RoleId",userModel.RoleId.ToString()),
                        new Claim("Role",userModel.Role),
                        new Claim("Operator",userModel.OperatorId.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(expiryTimeSpan),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)

            };

            // Generate token using JwtSecurityTokenHandler.
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwToken = jwtHandler.CreateJwtSecurityToken(securityTokenDescription);
            return jwtHandler.WriteToken(jwToken);
        }
    
    
    }
}
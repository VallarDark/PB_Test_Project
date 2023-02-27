using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Services.UserAgregate
{
    public class UserTokenProvider : IUserTokenProvider
    {
        private const string INVALIDE_TOKEN_EXCEPTION = "Token is invalid";
        private const int TOKEN_LIFE_TIME_MINUTES = 15;

        private readonly IConfiguration _configuration;

        public UserTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(JwtRegisteredClaimNames.Name, EncodeData(user.PersonalData.Name)),
                        new Claim(JwtRegisteredClaimNames.FamilyName, EncodeData(user.PersonalData.LastName)),
                        new Claim(JwtRegisteredClaimNames.Email, EncodeData(user.PersonalData.Email)),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),

                Expires = DateTime.UtcNow.AddMinutes(TOKEN_LIFE_TIME_MINUTES),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public PersonalData ReadToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Jwt:Audience"];
            validationParameters.ValidIssuer = _configuration["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var _);

            var email = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value;
            var name = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value;
            var lastName = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName).Value;

            return new PersonalData(
                DecodeData(email),
                DecodeData(name),
                DecodeData(lastName));
        }

        private string EncodeData(string data)
        {
            var textBytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(textBytes);
        }

        private string DecodeData(string data)
        {
            var base64EncodedBytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}

using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.UserAgregate
{
    public class UserTokenProvider : IUserTokenProvider
    {
        private const string INVALIDE_TOKEN_EXCEPTION = "Token is invalid";
        private const int TOKEN_LIFE_TIME_MINUTES = 5;

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
                        new Claim(JwtRegisteredClaimNames.Name, EncodingUtils.EncodeData(user.PersonalData.Name)),
                        new Claim(JwtRegisteredClaimNames.FamilyName, EncodingUtils.EncodeData(user.PersonalData.LastName)),
                        new Claim(JwtRegisteredClaimNames.Email, EncodingUtils.EncodeData(user.PersonalData.Email)),
                        new Claim("role", user.Role.ToString()),
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

        public ClaimsPrincipal? ReadToken(string token, TokenValidationParameters parameters, out SecurityToken? securityToken)
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

            return tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        }
    }
}

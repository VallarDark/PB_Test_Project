using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim(
                         ClaimType.Name.ToString(),
                         EncodingUtils.EncodeData(user.PersonalData.Name)),
                     new Claim(
                         ClaimType.LastName.ToString(),
                         EncodingUtils.EncodeData(user.PersonalData.LastName)),
                     new Claim(
                         ClaimType.Email.ToString(),
                         EncodingUtils.EncodeData(user.PersonalData.Email)),
                     new Claim(
                         ClaimType.Role.ToString(),
                         EncodingUtils.EncodeData(
                             Enum.GetName(typeof(UserRoleType),
                             user.Role.RoleType))),
                     new Claim(
                         ClaimType.Sid.ToString(),
                         EncodingUtils.EncodeData(user.SessionToken)),
                     new Claim(
                         ClaimType.PasswordHash.ToString(),
                         EncodingUtils.GetHashCode(user.Password))
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

        public ClaimsPrincipal? ReadToken(
            string token,
            TokenValidationParameters parameters,
            out SecurityToken? securityToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            return tokenHandler.ValidateToken(token, parameters, out securityToken);
        }
    }
}

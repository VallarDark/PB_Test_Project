using Domain.Aggregates.UserAggregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.UserAggregate
{
    public class UserTokenProvider : IUserTokenProvider
    {
        private const string INVALIDE_TOKEN_EXCEPTION = "Token is invalid";
        private const string INVALIDE_USER_DATA_EXCEPTION = "Invalid user data";
        private const int TOKEN_LIFE_TIME_MINUTES = 5;
        private const int REFRESH_TOKEN_LIFE_TIME_MINUTES = 60;

        private readonly string? _issuer;
        private readonly string? _audience;
        private readonly byte[] _key;

        public UserTokenProvider(IConfiguration configuration)
        {
            _issuer = configuration["JWT:Issuer"];
            _audience = configuration["JWT:Audience"];
            _key = EncodingUtils.AltDataEncoding.GetBytes(
                configuration["JWT:Key"] ?? string.Empty);
        }

        public TokenDto? GenerateToken(User user)
        {
            EnsuredUtils.EnsureNotNull(user, INVALIDE_USER_DATA_EXCEPTION);

            var claimsData = new ClaimsData(user);

            EnsuredUtils.EnsureNotNull(claimsData);

            var refreshTokenExpires = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_LIFE_TIME_MINUTES);

            return new TokenDto
            {
                Token = GenerateToken(claimsData),
                RefreshToken = claimsData.GenerateRefreshToken(refreshTokenExpires)
            };

        }

        public ClaimsData? ReadToken(
            string token,
            TokenValidationParameters parameters,
            out SecurityToken? securityToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            var claims = tokenHandler.ValidateToken(token, parameters, out securityToken);

            return new ClaimsData(claims);
        }

        public TokenDto RefreshToken(TokenDto tokenData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            if (!tokenHandler.CanReadToken(tokenData.Token))
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            var claims = tokenHandler.ValidateToken(
                tokenData.Token,
                validationParameters,
                out var _);

            var claimsData = new ClaimsData(claims);

            EnsuredUtils.EnsureNotNull(claimsData);

            try
            {
                if (!ClaimsData.IsRefreshTokenValid(tokenData.RefreshToken, claimsData))
                {
                    throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
                }
            }
            catch
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            return new TokenDto
            {
                Token = GenerateToken(claimsData),
                RefreshToken = tokenData.RefreshToken
            };
        }

        private string? GenerateToken(ClaimsData claimsData)
        {
            var encryptedClaimsData = claimsData.EncryptData();

            var currentTime = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimType.Name.ToString(), encryptedClaimsData.Name),
                     new Claim(ClaimType.LastName.ToString(), encryptedClaimsData.LastName),
                     new Claim(ClaimType.Email.ToString(), encryptedClaimsData.Email),
                     new Claim(ClaimType.Role.ToString(), encryptedClaimsData.RoleType),
                     new Claim(ClaimType.Sid.ToString(), encryptedClaimsData.SessionToken),
                     new Claim(ClaimType.PasswordHash.ToString(), encryptedClaimsData.Password)
                }),

                Expires = currentTime.AddMinutes(TOKEN_LIFE_TIME_MINUTES),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

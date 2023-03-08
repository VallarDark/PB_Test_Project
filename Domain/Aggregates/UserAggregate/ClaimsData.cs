using Domain.Exceptions;
using Domain.Utils;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Domain.Aggregates.UserAggregate
{
    public class ClaimsData
    {
        private const string TOKEN_SEPARATOR = "|_|_|";
        private const string INVALIDE_TOKEN_EXCEPTION = "Refresh token is invalid";
        private const string INVALIDE_REFRESH_TOKEN_EXCEPTION = "Refresh token is invalid";
        private const string REFRESH_TOKEN_EXPIRED_EXCEPTION = "Refresh token has been expired";

        public const int REFRESH_TOKEN_LIFE_TIME_MINUTES = 60;

        private bool isEncrypted;

        public bool IsEncrypted => isEncrypted;

        public string Name { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string SessionToken { get; private set; }

        public string RoleType { get; private set; }

        public string Password { get; private set; }

        public ClaimsPrincipal? Principal { get; private set; }

        public ClaimsData(User? user)
        {
            EnsuredUtils.EnsureNotNull(user);

            isEncrypted = false;

            Name = user.PersonalData.Name;
            LastName = user.PersonalData.LastName;
            Email = user.PersonalData.Email;
            SessionToken = user.SessionToken;
            RoleType = Enum.GetName(typeof(UserRoleType), user.Role.RoleType);
            Password = EncodingUtils.GetHashCode(user.Password);
        }

        public ClaimsData(
            string name,
            string lastName,
            string email,
            string sessionToken,
            string roleType,
            string password,
            bool isEncrypted)
        {
            Name = EnsuredUtils.EnsureStringIsNotEmpty(name);
            LastName = EnsuredUtils.EnsureStringIsNotEmpty(lastName);
            Email = EnsuredUtils.EnsureStringIsNotEmpty(email);
            SessionToken = EnsuredUtils.EnsureStringIsNotEmpty(sessionToken);
            RoleType = EnsuredUtils.EnsureStringIsNotEmpty(roleType);
            Password = EnsuredUtils.EnsureStringIsNotEmpty(password);
            this.isEncrypted = isEncrypted;
        }

        public ClaimsData(ClaimsPrincipal? claims)
        {
            EnsuredUtils.EnsureNotNull(claims);

            isEncrypted = true;

            Email = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.Email.ToString())?.Value);

            Name = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.Name.ToString())?.Value);

            LastName = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.LastName.ToString())?.Value);

            SessionToken = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.Sid.ToString())?.Value);

            RoleType = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.Role.ToString())?.Value);

            Password = EnsuredUtils.EnsureStringIsNotEmpty(
                claims.Claims.FirstOrDefault(c => c.Type == ClaimType.PasswordHash.ToString())?.Value);

            Principal = claims;
        }

        public ClaimsData EncryptData()
        {
            if (isEncrypted)
            {
                return this;
            }

            return new ClaimsData(
                EncodingUtils.EncodeData(Name),
                EncodingUtils.EncodeData(LastName),
                EncodingUtils.EncodeData(Email),
                EncodingUtils.EncodeData(SessionToken),
                EncodingUtils.EncodeData(RoleType),
                EncodingUtils.EncodeData(Password),
                true
                );
        }

        public ClaimsData DecryptData()
        {
            if (!isEncrypted)
            {
                return this;
            }

            return new ClaimsData(
                EncodingUtils.DecodeData(Name),
                EncodingUtils.DecodeData(LastName),
                EncodingUtils.DecodeData(Email),
                EncodingUtils.DecodeData(SessionToken),
                EncodingUtils.DecodeData(RoleType),
                EncodingUtils.DecodeData(Password),
                false);
        }

        public string GenerateRefreshToken()
        {
            var validUntilTermTotalMinutes = GetTotalMinutesNow();

            return GenerateRefreshToken(validUntilTermTotalMinutes);
        }

        public string GenerateRefreshToken(int validUntilTerm)
        {
            var claims = this;

            if (isEncrypted)
            {
                claims = DecryptData();
            }

            var validUntilTermString = validUntilTerm.ToString();

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(claims.Name);
            stringBuilder.Append(claims.LastName);
            stringBuilder.Append(claims.Email);
            stringBuilder.Append(claims.SessionToken);
            stringBuilder.Append(claims.RoleType);
            stringBuilder.Append(claims.Password);
            stringBuilder.Append(validUntilTermString);

            var data = stringBuilder.ToString();
            stringBuilder.Clear();

            stringBuilder.Append(EncodingUtils.GetHashCode(data));
            stringBuilder.Append(TOKEN_SEPARATOR);
            stringBuilder.Append(EncodingUtils.EncodeData(validUntilTermString));

            return stringBuilder.ToString();
        }

        public static Guid ValidateRefreshToken(
            string refreshToken,
            ClaimsData? claims,
            ILogger? logger = null)
        {
            if (claims == null)
            {
                throw new InvalidTokenException(INVALIDE_TOKEN_EXCEPTION);
            }

            int validTermTotalMinutes = 0;

            try
            {
                var validTermPosition = refreshToken.IndexOf(TOKEN_SEPARATOR) + TOKEN_SEPARATOR.Length;

                var validTermString = refreshToken.Substring(validTermPosition);

                var decodedValidTermString = EncodingUtils.DecodeData(validTermString);

                validTermTotalMinutes = int.Parse(decodedValidTermString);
            }
            catch (Exception ex)
            {
                logger?.Error(ex.Message);
                throw new InvalidRefreshTokenException(INVALIDE_REFRESH_TOKEN_EXCEPTION);
            }

            var validToken = claims.GenerateRefreshToken(validTermTotalMinutes);

            if (!validToken.Equals(refreshToken))
            {
                logger?.Error($"Valid token not equal to refresh" +
                    $" \n refresh: {refreshToken} valid: {validToken}" +
                    $" \n claims:{JsonConvert.SerializeObject(claims)} ");

                throw new InvalidRefreshTokenException(INVALIDE_REFRESH_TOKEN_EXCEPTION);
            }

            var totalMinutesNow = GetTotalMinutesNow();

            if (totalMinutesNow - REFRESH_TOKEN_LIFE_TIME_MINUTES > validTermTotalMinutes)
            {
                throw new InvalidRefreshTokenException(REFRESH_TOKEN_EXPIRED_EXCEPTION);
            }

            return default;
        }

        private static int GetTotalMinutesNow()
        {
            return (int)DateTime.UtcNow.Subtract(DateTime.MinValue).TotalMinutes;
        }
    }
}

using Domain.Utils;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Aggregates.UserAggregate
{
    public class ClaimsData
    {
        private const string TOKEN_SEPARATOR = "|_|_|";

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

        public string GenerateRefreshToken(DateTime validUntilTerm)
        {
            var claims = this;

            if (isEncrypted)
            {
                claims = DecryptData();
            }

            var validUntilTermString = validUntilTerm.ToString("MM/dd/yyyy h:mm tt");

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(claims.Name);
            stringBuilder.AppendLine(claims.LastName);
            stringBuilder.AppendLine(claims.Email);
            stringBuilder.AppendLine(claims.SessionToken);
            stringBuilder.AppendLine(claims.RoleType);
            stringBuilder.AppendLine(claims.Password);
            stringBuilder.Append(validUntilTermString);

            var data = stringBuilder.ToString();

            using var hashManager = SHA256.Create();

            var hash = hashManager.ComputeHash(
                EncodingUtils.AltDataEncoding.GetBytes(data));

            stringBuilder.Clear();

            foreach (var b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            stringBuilder.AppendLine(TOKEN_SEPARATOR);
            stringBuilder.Append(EncodingUtils.EncodeData(validUntilTermString));

            return stringBuilder.ToString();
        }

        public static bool IsRefreshTokenValid(string refreshToken, ClaimsData claims)
        {
            var validTermPosition = refreshToken.IndexOf(TOKEN_SEPARATOR) + TOKEN_SEPARATOR.Length;

            var validTermString = refreshToken.Substring(validTermPosition);

            var decodedValidTermString = EncodingUtils.DecodeData(validTermString);

            var validTerm = DateTime.Parse(decodedValidTermString);

            var validToken = claims.GenerateRefreshToken(validTerm);

            return validToken.Equals(refreshToken) && DateTime.UtcNow < validTerm;
        }
    }
}

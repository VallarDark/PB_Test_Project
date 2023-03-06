using Microsoft.IdentityModel.Tokens;

namespace Domain.Aggregates.UserAggregate
{
    public interface IUserTokenProvider
    {
        TokenDto? GenerateToken(User user);

        ClaimsData? ReadToken(
            string token,
            TokenValidationParameters parameters,
            out SecurityToken? securityToken);

        TokenDto RefreshToken(TokenDto tokenData);
    }
}

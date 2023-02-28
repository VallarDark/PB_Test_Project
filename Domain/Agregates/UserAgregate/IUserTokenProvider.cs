using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Domain.Agregates.UserAgregate
{
    public interface IUserTokenProvider
    {
        string GenerateToken(User user);

        ClaimsPrincipal? ReadToken(string token, TokenValidationParameters parameters, out SecurityToken? securityToken);
    }
}

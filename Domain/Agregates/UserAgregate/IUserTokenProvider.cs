namespace Domain.Agregates.UserAgregate
{
    public interface IUserTokenProvider
    {
        string GenerateToken(User user);

        PersonalData ReadToken(string token);
    }
}

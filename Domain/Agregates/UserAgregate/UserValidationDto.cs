namespace Domain.Agregates.UserAgregate
{
    public class UserValidationDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string SessionToken { get; set; }

        public string PasswordHash { get; set; }

        public UserRoleType Role { get; set; }
    }
}

namespace Domain.Agregates.UserAgregate
{
    public class UserDto
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public UserRoleDto Role { get; set; }
    }
}

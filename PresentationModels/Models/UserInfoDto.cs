using Contracts;
using Domain.Aggregates.UserAggregate;

namespace PresentationModels.Models
{
    public class UserInfoDto
    {
        public TokenDto? TokenDto { get; set; }

        public string? UserName { get; set; }

        public UserRoleType? UserRole { get; set; }

        public RepositoryType? RepositoryType { get; set; }
    }
}

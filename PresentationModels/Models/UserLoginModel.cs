using Domain.Aggregates.UserAggregate;
using System.ComponentModel.DataAnnotations;

namespace PresentationModels.Models
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        [MinLength(User.MIN_LENGHT)]
        public string Email { get; set; }

        [Required]
        [MinLength(User.PASSWORD_MIN_LENGHT)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

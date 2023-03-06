using System.ComponentModel.DataAnnotations;

namespace PresentationModels.Models
{
    public class RefreshTokenModel
    {
        [Required(ErrorMessage = "Token is required")]
        [MinLength(1)]
        public string Token { get; set; }

        [Required(ErrorMessage = "RefreshToken is required")]
        [MinLength(1)]
        public string RefreshToken { get; set; }
    }
}

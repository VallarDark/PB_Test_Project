﻿using Domain.Aggregates.UserAggregate;
using System.ComponentModel.DataAnnotations;

namespace PresentationModels.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(User.PASSWORD_MIN_LENGHT)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(User.PASSWORD_MIN_LENGHT)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(User.PASSWORD_MIN_LENGHT)]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(User.PASSWORD_MIN_LENGHT)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
    }
}

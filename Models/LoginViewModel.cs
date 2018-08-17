using System;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "Please type your email")]
        [EmailAddress]
        public string login_Email {get; set;}

        [Required(ErrorMessage = "Please type your password")]
        public string login_Password {get; set;}

    }
}
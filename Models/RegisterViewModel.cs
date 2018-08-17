using System;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please type your first name")]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage = "Letter only")]
        public string first_name{get; set;}

        [Required(ErrorMessage = "Please type your last name")]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage = "Letter only")]
        public string last_name{get; set;}

        [Required(ErrorMessage = "Please type your email")]
        [EmailAddress]
        public string register_email {get; set;}

        [Required (ErrorMessage = "Please type your password")]
        [RegularExpression("^((?=.*?[a-zA-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",ErrorMessage="At least one letter, one number,one special character and at least 8 charaters")]
        public string register_password {get; set;}

        [Required(ErrorMessage = "Please type your password again")]
        [Compare("register_password", ErrorMessage=" Password should match")]
        public string confirm {get;set;}
    }
}
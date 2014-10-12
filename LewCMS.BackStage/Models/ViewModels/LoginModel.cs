using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Invalid Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
}
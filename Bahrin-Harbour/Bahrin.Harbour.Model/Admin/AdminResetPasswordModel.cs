using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PuthaganModel.Admin
{
    public class AdminResetPasswordModel
    {
        public Guid adminId { get; set; }
        [Display(Name = "Old password")]
        public string oldPassword { get; set; }
        [Display(Name = "New password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public string passCode { get; set; }
        public bool fromEmail { get; set; }
    }
}

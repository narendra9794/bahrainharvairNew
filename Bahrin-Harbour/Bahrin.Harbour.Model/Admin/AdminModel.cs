using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Bahrin.Harbour.Model.Admin
{
    public class AdminModel
    {
        public Guid _id { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        public DateTime dateofBirth { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string email { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [Display(Name = "Phone number")]
        public string phoneNumber { get; set; }
        public IFormFile ProfileImage { get; set; }

    }
}

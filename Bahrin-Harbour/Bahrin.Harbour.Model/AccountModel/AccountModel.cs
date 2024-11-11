using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bahrin.Harbour.Model.AccountModel
{

    public class SignInModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        public Int32 localTimeZoneOffset { get; set; }
        public bool rememberMe { get; set; }
        public bool isUpdate { get; set; }
    }

    public class ForgetPasswordModel()
    {
        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Please Enter The New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter The Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
    
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter The Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
    
    public class ResetNewPasswordModel
    {


        [Required(ErrorMessage = "Please Enter The Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please Enter The New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter The Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class AdminSigninResponse
    {
        public AdminModel data { get; set; }
        public StatusModel status { get; set; }
    }

    public class StatusModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Int32 totalRecords { get; set; }
        public List<string?> roles { get; set; }
    }

      public class ResetPasswordResponse
      {
        public bool status { get; set; }
        public string message { get; set; }
        public string otp { get; set; }
        public string token { get; set; }
        public string UserId { get; set; }
       
      }



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
        public string image { get; set; }
        public string deviceToken { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public Guid createdBy { get; set; }
        public Guid modifiedBy { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public string password { get; set; }
        public string passCode { get; set; }

        //[Required(ErrorMessage = "Please choose profile image")]
        //[Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        public bool viewOnly { get; set; }
        public string OldEmail { get; set; }
        public bool isSuperAdmin { get; set; }
        public int localTimeZoneOffset { get; set; }

    }


    public class AdminUserModel
    {
        public Guid? id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }
        public string? Role { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Pin { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageFileName { get; set; }
        public string? ImageFolderName { get; set; }
        public string? ImageLink { get; set; }
        public bool IsActive { get; set; }
    }

}



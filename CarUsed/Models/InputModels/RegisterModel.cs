using System.ComponentModel.DataAnnotations;

namespace CarUsed.InputModels
{
    public class RegisterModel
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare(nameof(Password),ErrorMessage ="confirm password miss match")]
        public string ConfirmePassword { get; set; }
    }
}

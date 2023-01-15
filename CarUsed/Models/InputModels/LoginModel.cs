using System.ComponentModel.DataAnnotations;

namespace CarUsed.Models.InputModels
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage ="input is not Email"),StringLength(25,MinimumLength =5,ErrorMessage ="email length mustbe 5-25")]
        public string Email { get; set; }

        [RegularExpression("^[A-Z-a-z0-9]+$",ErrorMessage ="pass should be A-Z,a-z,0-9"),StringLength(25,MinimumLength =5,ErrorMessage ="length musbe 5-25")]
        public string Password { get; set; }
        //compare
    }
}

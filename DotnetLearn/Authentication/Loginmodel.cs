using System.ComponentModel.DataAnnotations;

namespace DotnetLearn.Authentication
{
    public class Loginmodel
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string Username { get; set;}

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set;}
    }
}

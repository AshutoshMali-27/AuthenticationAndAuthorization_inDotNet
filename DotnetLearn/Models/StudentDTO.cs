using DotnetLearn.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DotnetLearn.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage ="Student name is Required")]
        [StringLength(30)]
        public string StudentName { get; set; }

        [EmailAddress(ErrorMessage ="Please Enter valid Email Address")]
        public string Email { get; set; }

        //[Range(10,20)]
        //public int Age { get; set; }

        [Required(ErrorMessage ="Student address is required")]
        public string Address { get; set; }

        //public string Password {  get; set; }

        //[Compare(nameof(Password))]
        //public string ConfirmPassword { get; set; }

        //[DateCheck]
        //public DateTime AddmissionDate { get; set; }
    }
}

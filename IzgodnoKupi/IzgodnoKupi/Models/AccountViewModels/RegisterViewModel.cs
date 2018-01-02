using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [Display(Name = "Паролата")]
        [StringLength(100, ErrorMessage = "{0} трябва да има поне {2} и максимум {1} символа дължина.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}

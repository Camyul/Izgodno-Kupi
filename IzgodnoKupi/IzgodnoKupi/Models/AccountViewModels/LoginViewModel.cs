using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола:")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }
}

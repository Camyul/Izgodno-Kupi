using IzgodnoKupi.Common;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.RoleViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [Display(Name = "Име")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string RoleName { get; set; }
    }
}

using IzgodnoKupi.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Роля")]
        public string ApplicationRoleId { get; set; }
    }
}

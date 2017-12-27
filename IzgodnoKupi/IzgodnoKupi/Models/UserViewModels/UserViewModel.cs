using IzgodnoKupi.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.UserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Role")]
        public string ApplicationRoleId { get; set; }
    }
}

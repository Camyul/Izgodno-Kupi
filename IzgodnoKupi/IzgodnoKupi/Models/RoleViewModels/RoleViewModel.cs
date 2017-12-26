using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.RoleViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Име")]
        public string RoleName { get; set; }
    }
}

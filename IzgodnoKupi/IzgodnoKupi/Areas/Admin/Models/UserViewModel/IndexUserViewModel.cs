using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.UserViewModels;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.UserViewModel
{
    public class IndexUserViewModel
    {
        public ICollection<UserListViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}

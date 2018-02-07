using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.ProductViewModels;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.Product
{
    public class IndexAdminPageViewModel
    {
        public ICollection<ProductViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}

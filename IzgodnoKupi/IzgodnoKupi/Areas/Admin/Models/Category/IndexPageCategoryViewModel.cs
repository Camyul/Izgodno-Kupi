using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.Category
{
    public class IndexPageCategoryViewModel
    {
        public ICollection<CategoryViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}

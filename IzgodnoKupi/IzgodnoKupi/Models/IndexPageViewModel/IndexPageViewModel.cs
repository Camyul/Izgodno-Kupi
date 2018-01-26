using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.ProductViewModels;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Models.IndexPageViewModel
{
    public class IndexPageViewModel
    {
        public ICollection<PreviewProductViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}

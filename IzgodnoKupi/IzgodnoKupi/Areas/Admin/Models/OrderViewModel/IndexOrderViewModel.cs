using IzgodnoKupi.Web.Extensions;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel
{
    public class IndexOrderViewModel
    {
        public ICollection<OrderListViewModel> Items { get; set; }
        public Pager Pager { get; set; }
    }
}

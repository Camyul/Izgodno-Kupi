using IzgodnoKupi.Data.Model.Abstracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class Picture : DataModel
    {
        public string ImageUrl { get; set; }

        public string ThumbImageUrl { get; set; }

        public string Title { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}

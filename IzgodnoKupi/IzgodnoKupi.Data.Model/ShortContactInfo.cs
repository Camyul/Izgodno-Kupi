using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Data.Model
{
    public class ShortContactInfo : DataModel
    {
        private ICollection<Order> orders;

        public ShortContactInfo()
        {
            this.Orders = new HashSet<Order>();
        }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PhoneRegex)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Order> Orders
        {
            get
            {
                return this.orders;
            }
            set
            {
                this.orders = value;
            }
        }
    }
}

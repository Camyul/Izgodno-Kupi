using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class FullContactInfo : DataModel
    {
        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual User User { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PhoneRegex)]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(ValidationConstants.AddressMinLength)]
        [MaxLength(ValidationConstants.AddressMaxLength)]
        [RegularExpression(ValidationConstants.EnBgDigitSpaceMinus)]
        public string Address { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string City { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinusDot)]
        public string Area { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.PostCodeMaxLength)]
        public string PostCode { get; set; }

        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string CompanyName { get; set; }

        [MinLength(ValidationConstants.EIKMinLength)]
        [MaxLength(ValidationConstants.EIKMaxLength)]
        public string EIK { get; set; }

        [MinLength(ValidationConstants.EIKMinLength)]
        [MaxLength(ValidationConstants.EIKMaxLength)]
        public string BGEIK { get; set; }

        [MinLength(ValidationConstants.AddressMinLength)]
        [MaxLength(ValidationConstants.AddressMaxLength)]
        public string CompanyAddress { get; set; }

        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string CompanyCity { get; set; }

        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        public string MOL { get; set; }

        public string Note { get; set; }

        public Courier Courier { get; set; }

        [MinLength(ValidationConstants.StandardMinLength)]
        [MaxLength(ValidationConstants.NameMaxLength)]
        public string OfficeName { get; set; }
    }
}

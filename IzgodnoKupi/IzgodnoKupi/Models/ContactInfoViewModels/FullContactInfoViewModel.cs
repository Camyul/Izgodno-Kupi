using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.ContactInfoViewModels
{
    public class FullContactInfoViewModel
    {
        public FullContactInfoViewModel()
        {
        }

        public FullContactInfoViewModel(FullContactInfo contactInfo)
        {
            this.FirstName = contactInfo.FirstName;
            this.LastName = contactInfo.LastName;
            this.PhoneNumber = contactInfo.PhoneNumber;
            this.Address = contactInfo.Address;
            this.City = contactInfo.City;
            this.Area = contactInfo.Area;
            this.PostCode = contactInfo.PostCode;
            this.CompanyName = contactInfo.CompanyName;
            this.EIK = contactInfo.EIK;
            this.BGEIK = contactInfo.BGEIK;
            this.CompanyCity = contactInfo.CompanyCity;
            this.CompanyAddress = contactInfo.CompanyAddress;
            this.MOL = contactInfo.MOL;
            this.Note = contactInfo.Note;
        }

        [Required]
        [Display(Name = "Име")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "GSM")]
        [RegularExpression(ValidationConstants.PhoneRegex, ErrorMessage = ValidationConstants.PhoneErrorMessage)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        [MinLength(ValidationConstants.AddressMinLength, ErrorMessage = ValidationConstants.AddressMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.AddressMaxLength, ErrorMessage = ValidationConstants.AddressMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgDigitSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Населено Място")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Област")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinusDot, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string Area { get; set; }

        [Required]
        [Display(Name = "Пощенски код")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.PostCodeMaxLength, ErrorMessage = ValidationConstants.PostCodeMaxLengthErrorMessage)]
        public string PostCode { get; set; }

        [Display(Name = "Име на фирмата")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        public string CompanyName { get; set; }

        [Display(Name = "ЕИК по БУЛСТАТ")]
        [MinLength(ValidationConstants.EIKMinLength, ErrorMessage = ValidationConstants.EIKMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.EIKMaxLength, ErrorMessage = ValidationConstants.EIKMaxLengthErrorMessage)]
        public string EIK { get; set; }

        [Display(Name = "Идентификационен номер по ДДС")]
        [MinLength(ValidationConstants.EIKMinLength, ErrorMessage = ValidationConstants.EIKMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.EIKMaxLength, ErrorMessage = ValidationConstants.EIKMaxLengthErrorMessage)]
        public string BGEIK { get; set; }

        [Display(Name = "Адрес по регистрация")]
        [MinLength(ValidationConstants.AddressMinLength, ErrorMessage = ValidationConstants.AddressMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.AddressMaxLength, ErrorMessage = ValidationConstants.AddressMaxLengthErrorMessage)]
        public string CompanyAddress { get; set; }

        [Display(Name = "Населено място")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        public string CompanyCity { get; set; }

        [Display(Name = "МОЛ")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.NameMaxLength, ErrorMessage = ValidationConstants.NameMaxLengthErrorMessage)]
        public string MOL { get; set; }

        [Display(Name = "Коментар")]
        public string Note { get; set; }
    }
}

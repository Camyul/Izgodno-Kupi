namespace IzgodnoKupi.Common
{
    public class ValidationConstants
    {
        // Don't mach special symbols and digit, but mach "space" and "-"
        public const string EnBgSpaceMinus = @"^[a-zA-Zа-яА-Я\s\-]+$";

        public const string EnBgSpaceMinusDot = @"^[a-zA-Zа-яА-Я\s\-\.]+$";

        // Don't mach special symbols, but mach "space" and "-"
        public const string EnBgDigitSpaceMinus = @"^[a-zA-Zа-яА-Я0-9\s\-\.]+$";

        //public const string EnBgDigitSpaceMinusDot = @"^[a-zA-Zа-яА-Я0-9\s\-]+$";

        // Don't mach special symbols "<", ">", "|", "/", "\", '$'
        public const string DescriptionRegex = @"^[a-zA-Zа-яА-Я0-9\s\-\.,!():;?/+_%@""'#&=\*]+$";

        //// sourse http://stackoverflow.com/questions/8908976/c-sharp-regex-to-validate-phone-number
        public const string PhoneRegex = @"\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})";

        //// source https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx
        //public const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        //// source http://stackoverflow.com/questions/5717312/regular-expression-for-url
        //public const string UrlRegex = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";

        public const int StandardMinLength = 3;
        public const int StandartMaxLength = 200;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 150;
        public const int LongDescriptionMaxLength = 2700;

        public const int AddressMinLength = 2;
        public const int AddressMaxLength = 100;

        public const int ImageUrlMinLength = 6;
        public const int ImageUrlMaxLength = 300;

        public const int QuantityMinValue = 1;
        public const int QuantityMaxValue = 10000000;

        public const double PriceMinValue = 0.01;
        public const double PriceMaxValue = 99999.99;

        public const double DiscountMinValue = 0.00d;
        public const double DiscountMaxValue = 999.99d;

        public const string MinLengthUrlErrorMessage = "Линка към снимката трябва да бъде поне 6 символа";
        public const string MaxLengthUrlErrorMessage = "Линка към снимката може да бъде максимум 300 символа";

        public const string MinLengthFieldErrorMessage = "Полето {0} трябва да бъде поне 3 символа";
        public const string MaxLengthFieldErrorMessage = "Полето {0} може да бъде максимум 200 символа";

        public const string MinLengthDescriptionErrorMessage = "Описанието трябва да бъде поне 10 символа";
        public const string MaxLengthDescriptionErrorMessage = "Описанието може да бъде максимум 150 символа";
        public const string MaxLengthLongDescriptionErrorMessage = "Описанието може да бъде максимум 2800 символа";

        public const string NotAllowedSymbolsErrorMessage = "Полето {0} съдържа неразрешени символи";

        public const string QuаntityOutOfRangeErrorMessage = "Количеството не може да бъде отрицателно или нула";
        public const string PriceOutOfRangeErrorMessage = "{0} Невалиднa цена, диапазон 0.01, 99 999.99.";
        public const string DiscountOutOfRangeErrorMessage = "{0} Невалиднa отстъпка, диапазон 0.00%, 999.99%.";
    }
}

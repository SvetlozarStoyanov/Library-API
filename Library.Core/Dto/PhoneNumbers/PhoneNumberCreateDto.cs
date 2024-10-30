using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.PhoneNumbers
{
    public class PhoneNumberCreateDto
    {
        [Phone]
        [Required]
        public string Number { get; set; }
        [EnumDataType(typeof(PhoneNumberType))]
        public PhoneNumberType Type { get; set; }
        public long CountryId { get; set; }
        public bool IsMain { get; set; }
    }
}

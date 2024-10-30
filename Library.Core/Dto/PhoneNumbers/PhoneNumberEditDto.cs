using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.PhoneNumbers
{
    public class PhoneNumberEditDto
    {
        public long Id { get; init; }
        [Required]
        [Phone]
        public string Number { get; set; }
        public long CountryId { get; set; }
        [EnumDataType(typeof(PhoneNumberType))]
        public PhoneNumberType Type { get; set; }
        public bool IsMain { get; set; }
    }
}

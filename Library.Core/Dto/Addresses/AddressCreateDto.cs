using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Addresses
{
    public class AddressCreateDto
    {
        [Required]
        public long CountryId { get; set; }
        [Required]
        [MinLength(4), MaxLength(170)]
        public string City { get; set; }
        [Required]
        [MinLength(4), MaxLength(170)]
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        [EnumDataType(typeof(AddressType))]
        public AddressType Type { get; set; }
    }
}

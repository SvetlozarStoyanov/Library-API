using Library.Core.Dto.Countries;

namespace Library.Core.Dto.Addresses
{
    public class AddressListDto
    {
        public long Id { get; init; }
        public CountryNameDto Country { get; set; }
        public string City { get; init; }
        public string AddressLine { get; init; }
        public string Type { get; set; }
    }
}

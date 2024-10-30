using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class Address
    {
        public Address()
        {

        }

        public long Id { get; init; }
        public string City { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public AddressType Type { get; set; }
        public AddressStatus Status { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
    }
}

using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class PhoneNumber
    {
        public PhoneNumber()
        {

        }

        public long Id { get; init; }
        public string Number { get; set; }
        public PhoneNumberType Type { get; set; }
        public PhoneNumberStatus Status { get; set; }
        public bool IsMain { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
    }
}

using Library.Core.Dto.PhoneNumbers;

namespace Library.Core.Dto.Clients
{
    public class ClientPhoneNumbersEditDto
    {
        public ClientPhoneNumbersEditDto()
        {
            PhoneNumbers = new HashSet<PhoneNumberCreateOrEditDto>();
        }

        public long ClientId { get; set; }
        public IEnumerable<PhoneNumberCreateOrEditDto> PhoneNumbers { get; set; }
    }
}

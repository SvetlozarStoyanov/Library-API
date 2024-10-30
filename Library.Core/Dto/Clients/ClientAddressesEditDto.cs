using Library.Core.Dto.Addresses;

namespace Library.Core.Dto.Clients
{
    public class ClientAddressesEditDto
    {
        public long ClientId { get; init; }
        public ICollection<AddressCreateOrEditDto> Addresses { get; set; }
    }
}

using Library.Core.Dto.Clients;

namespace Library.Core.Dto.ClientCards
{
    public class ClientCardNestedListDto
    {
        public long Id { get; init; }
        public ClientNestedListDto Client { get; set; }
    }
}

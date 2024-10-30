using Library.Core.Dto.Books;
using Library.Core.Dto.ClientCards;

namespace Library.Core.Dto.Checkouts
{
    public class CheckoutDetailsDto
    {
        public CheckoutDetailsDto()
        {

        }

        public long Id { get; init; }
        public ClientCardNestedListDto ClientCard { get; init; }
        public BookNestedListDto Book { get; init; }
        public DateTime CheckoutTime { get; init; }
        public DateTime DueTime { get; init; }
        public DateTime? ReturnTime { get; init; }
        public bool IsFinalized { get; init; }
    }
}

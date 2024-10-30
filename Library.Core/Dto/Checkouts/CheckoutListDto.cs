using Library.Core.Dto.Books;
using Library.Core.Dto.ClientCards;

namespace Library.Core.Dto.Checkouts
{
    public class CheckoutListDto
    {
        public long Id { get; set; }
        public ClientCardNestedListDto ClientCard { get; set; }
        public BookNestedListDto Book { get; set; }
        public DateTime CheckoutTime { get; set; }
        public DateTime DueTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public bool IsFinalized { get; set; }
    }
}

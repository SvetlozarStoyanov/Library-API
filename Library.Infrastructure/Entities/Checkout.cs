using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class Checkout
    {
        public Checkout()
        {
            Fines = new HashSet<Fine>();
        }

        public long Id { get; init; }
        public CheckoutStatus Status { get; set; }
        public DateTime CheckoutTime { get; set; }
        public DateTime DueTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public long ClientCardId { get; set; }
        public ClientCard ClientCard { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public ICollection<Fine> Fines { get; set; }
    }
}

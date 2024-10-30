using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class Fine
    {
        public Fine()
        {

        }

        public long Id { get; init; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public FineStatus Status { get; set; }
        public FineReason Reason { get; set; }
        public DateTime IssueDate { get; set; }
        public long CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
    }
}

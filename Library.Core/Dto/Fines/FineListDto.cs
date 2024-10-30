namespace Library.Core.Dto.Fines
{
    public class FineListDto
    {
        public long Id { get; init; }
        public long CheckoutId { get; init; }
        public decimal Amount { get; init; }
        public string Status { get; init; }
        public string Code { get; init; }
        public string Reason { get; init; }
        public DateTime IssueDate { get; init; }
    }
}

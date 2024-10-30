namespace Library.Core.Dto.Checkouts
{
    public class CheckoutCreateDto
    {
        public long BookId { get; set; }
        public DateTime CheckoutTime { get; set; }
        public long ClientCardId { get; set; }
    }
}

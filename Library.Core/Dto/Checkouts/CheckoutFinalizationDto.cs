namespace Library.Core.Dto.Checkouts
{
    public class CheckoutFinalizationDto
    {
        public long Id { get; set; }
        public bool BookIsReturned { get; set; }
        public DateTime Time { get; set; }
    }
}

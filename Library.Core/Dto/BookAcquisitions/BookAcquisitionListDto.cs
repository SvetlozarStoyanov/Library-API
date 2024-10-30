namespace Library.Core.Dto.BookAcquisitions
{
    public class BookAcquisitionListDto
    {
        public long Id { get; init; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
    }
}

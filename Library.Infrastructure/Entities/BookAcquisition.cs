using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class BookAcquisition
    {
        public BookAcquisition()
        {

        }

        public long Id { get; init; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }
        public BookAcquisitionType Type { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.BookAcquisitions
{
    public class BookAcquisitionRestockDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public DateTime Time { get; set; }
    }
}

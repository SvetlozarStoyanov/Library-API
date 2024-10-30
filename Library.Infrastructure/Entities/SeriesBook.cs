namespace Library.Infrastructure.Entities
{
    public class SeriesBook
    {
        public SeriesBook()
        {

        }

        public long Id { get; init; }
        public long SeriesId { get; set; }
        public Series Series { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
    }
}

namespace Library.Infrastructure.Entities
{
    public class GenreBook
    {
        public GenreBook()
        {

        }

        public long Id { get; init; }
        public long GenreId { get; set; }
        public Genre Genre { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
    }
}

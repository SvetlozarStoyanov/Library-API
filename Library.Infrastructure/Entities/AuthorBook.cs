namespace Library.Infrastructure.Entities
{
    public class AuthorBook
    {
        public AuthorBook()
        {

        }

        public long Id { get; init; }
        public long AuthorId { get; set; }
        public Author Author { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
    }
}

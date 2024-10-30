namespace Library.Infrastructure.Entities
{
    public class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        public long Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

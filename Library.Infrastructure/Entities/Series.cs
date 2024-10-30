namespace Library.Infrastructure.Entities
{
    public class Series
    {
        public Series()
        {
            Books = new HashSet<Book>();
        }

        public long Id { get; init; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

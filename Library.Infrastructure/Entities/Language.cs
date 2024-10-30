namespace Library.Infrastructure.Entities
{
    public class Language
    {
        public Language()
        {
            Books = new HashSet<Book>();
        }

        public long Id { get; init; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}

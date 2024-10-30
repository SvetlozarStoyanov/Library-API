namespace Library.Infrastructure.Entities
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        public long Id { get; init; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

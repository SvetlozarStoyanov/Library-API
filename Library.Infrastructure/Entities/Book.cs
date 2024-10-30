using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class Book
    {
        public Book()
        {
            Acquisitions = new HashSet<BookAcquisition>();
            Authors = new HashSet<Author>();
            Genres = new HashSet<Genre>();
            Series = new HashSet<Series>();
            Checkouts = new HashSet<Checkout>();
        }

        public long Id { get; init; }
        public string Title { get; set; }
        public string? ISBN { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public int Quantity { get; set; }
        public DateTime? PublicationDate { get; set; }
        public BookStatus Status { get; set; }
        public long LanguageId { get; set; }
        public Language Language { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
        public virtual ICollection<BookAcquisition> Acquisitions { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Series> Series { get; set; }
        public virtual ICollection<Checkout> Checkouts { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Books
{
    public class BooksFilterDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int ItemsPerPage { get; set; } = 6;
        public string? SearchTerm { get; set; }
        public IEnumerable<long>? AuthorIds { get; set; }
        public IEnumerable<long>? GenreIds { get; set; }
        public IEnumerable<long>? SeriesIds { get; set; }
        public IEnumerable<long>? CountryIds { get; set; }
    }
}

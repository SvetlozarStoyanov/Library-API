using Library.Core.Dto.Authors;
using Library.Core.Dto.Genres;
using Library.Core.Dto.Series;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Books
{
    public class BookExperimentalCreateDto
    {
        public BookExperimentalCreateDto()
        {
            Authors = new HashSet<AuthorCreateDto>();
            Genres = new HashSet<GenreCreateDto>();
            Series = new HashSet<SeriesCreateDto>();
        }

        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [MinLength(10), MaxLength(18)]
        public string? ISBN { get; set; }
        [Required]
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
        public long LanguageId { get; set; }
        public long CountryId { get; set; }
        [Range(1, int.MaxValue)]
        public int PageCount { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime? PublicationDate { get; set; }
        public IEnumerable<AuthorCreateDto> Authors { get; set; }
        public IEnumerable<GenreCreateDto> Genres { get; set; }
        public IEnumerable<SeriesCreateDto> Series { get; set; }
    }
}

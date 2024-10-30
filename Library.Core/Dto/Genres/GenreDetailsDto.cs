using Library.Core.Dto.Books;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Genres
{
    public class GenreDetailsDto
    {
        public GenreDetailsDto()
        {
            Books = new HashSet<BookNestedListDto>();
        }

        public long Id { get; init; }
        [MinLength(2), MaxLength(50)]
        public string Name { get; init; }
        public string Description { get; init; }
        public IEnumerable<BookNestedListDto> Books { get; init; }
    }
}

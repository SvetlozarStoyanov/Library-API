using Library.Core.Dto.Books;

namespace Library.Core.Dto.Genres
{
    public class GenreListDto
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public IEnumerable<BookNestedListDto> Books { get; init; }
    }
}

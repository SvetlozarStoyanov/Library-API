using Library.Core.Dto.Books;

namespace Library.Core.Dto.Series
{
    public class SeriesListDto
    {
        public long Id { get; init; }
        public string Title { get; init; }
        public IEnumerable<BookNestedListDto> Books { get; init; }
    }
}

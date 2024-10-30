using Library.Core.Dto.Books;

namespace Library.Core.Dto.Series
{
    public class SeriesDetailsDto
    {
        public SeriesDetailsDto()
        {
            Books = new HashSet<BookNestedListDto>();
        }

        public long Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public ICollection<BookNestedListDto> Books { get; init; }
    }
}

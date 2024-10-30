using Library.Core.Dto.Books;

namespace Library.Core.Dto.Authors
{
    public class AuthorListDto
    {
        public long Id { get; init; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; init; }
        public IEnumerable<BookNestedListDto> Books { get; init; }
    }
}

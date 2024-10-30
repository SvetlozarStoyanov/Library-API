using Library.Core.Dto.Books;

namespace Library.Core.Dto.Authors
{
    public class AuthorDetailsDto
    {
        public AuthorDetailsDto()
        {
            Books = new HashSet<BookNestedListDto>();
        }

        public long Id { get; init; }
        public string FirstName { get; init; }
        public string? MiddleName { get; init; }
        public string LastName { get; init; }
        public string Description { get; init; }
        public DateTime DateOfBirth { get; init; }
        public DateTime? DateOfDeath { get; init; }
        public IEnumerable<BookNestedListDto> Books { get; init; }
    }
}

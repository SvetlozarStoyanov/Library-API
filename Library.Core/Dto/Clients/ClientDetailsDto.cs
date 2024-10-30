namespace Library.Core.Dto.Clients
{
    public class ClientDetailsDto
    {
        public long Id { get; init; }
        public string FirstName { get; init; }
        public string? MiddleName { get; init; }
        public string LastName { get; init; }
        public string? UnifiedCivilNumber { get; init; }
        public DateTime DateOfBirth { get; init; }
    }
}

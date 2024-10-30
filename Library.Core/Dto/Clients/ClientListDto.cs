namespace Library.Core.Dto.Clients
{
    public class ClientListDto
    {
        public long Id { get; init; }
        public string FullName { get; init; }
        public string? UnifiedCivilNumber { get; init; }
        public DateTime DateOfBirth { get; init; }
    }
}

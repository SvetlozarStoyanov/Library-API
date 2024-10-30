namespace Library.Core.Dto.Emails
{
    public class EmailListDto
    {
        public long Id { get; init; }
        public long ClientId { get; init; }
        public string Address { get; init; }
        public string Type { get; init; }
        public bool IsMain { get; init; }
    }
}

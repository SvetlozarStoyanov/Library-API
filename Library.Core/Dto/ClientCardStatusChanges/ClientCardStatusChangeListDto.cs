namespace Library.Core.Dto.ClientCardStatusChanges
{
    public class ClientCardStatusChangeListDto
    {
        public long Id { get; init; }
        public string Reason { get; init; }
        public string Status { get; init; }
        public string UpdatedOn { get; init; }
    }
}

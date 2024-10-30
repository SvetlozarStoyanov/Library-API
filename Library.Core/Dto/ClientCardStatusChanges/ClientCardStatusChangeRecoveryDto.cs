namespace Library.Core.Dto.ClientCardStatusChanges
{
    public class ClientCardStatusChangeRecoveryDto
    {
        public long ClientCardId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool ReactivateClientCard { get; set; }
    }
}

namespace Library.Core.Dto.ClientCards
{
    public class ClientCardListDto
    {
        public long Id { get; set; }
        public long ClientCardTypeId { get; set; }
        public string ClientCardTypeName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
    }
}
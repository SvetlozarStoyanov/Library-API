namespace Library.Core.Dto.ClientCards
{
    public class ClientCardCreateDto
    {
        public long ClientId { get; set; }
        public long ClientCardTypeId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

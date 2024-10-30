namespace Library.Core.Dto.PhoneNumbers
{
    public class PhoneNumberListDto
    {
        public long Id { get; init; }
        public long ClientId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public bool IsMain { get; set; }
    }
}

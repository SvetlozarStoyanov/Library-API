using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class Email
    {
        public Email()
        {

        }

        public long Id { get; set; }
        public string Address { get; set; }
        public EmailType Type { get; set; }
        public EmailStatus Status { get; set; }
        public bool IsMain { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}

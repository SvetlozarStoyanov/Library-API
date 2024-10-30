using Library.Core.Dto.Emails;

namespace Library.Core.Dto.Clients
{
    public class ClientEmailsEditDto
    {
        public ClientEmailsEditDto()
        {
            Emails = new HashSet<EmailCreateOrEditDto>();
        }

        public long ClientId { get; set; }
        public IEnumerable<EmailCreateOrEditDto> Emails { get; set; }
    }
}

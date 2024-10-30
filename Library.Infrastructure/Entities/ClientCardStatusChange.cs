using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class ClientCardStatusChange
    {
        public ClientCardStatusChange()
        {

        }

        public long Id { get; init; }
        public DateTime UpdatedOn { get; set; }
        public ClientCardStatusChangeReason Reason { get; set; }
        public ClientCardStatus Status { get; set; }
        public long ClientCardId { get; set; }
        public ClientCard ClientCard { get; set; }
    }
}

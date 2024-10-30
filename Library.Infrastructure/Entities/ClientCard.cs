using Library.Infrastructure.Enums;

namespace Library.Infrastructure.Entities
{
    public class ClientCard
    {
        public ClientCard()
        {
            Checkouts = new HashSet<Checkout>();
            StatusChanges = new HashSet<ClientCardStatusChange>();
        }

        public long Id { get; init; }
        public ClientCardStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
        public long ClientCardTypeId { get; set; }
        public ClientCardType ClientCardType { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
        public ICollection<ClientCardStatusChange> StatusChanges { get; set; }
    }
}

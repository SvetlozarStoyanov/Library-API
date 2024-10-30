namespace Library.Infrastructure.Entities
{
    public class Client
    {
        public Client()
        {
            Addresses = new HashSet<Address>();
            ClientCards = new HashSet<ClientCard>();
            Emails = new HashSet<Email>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        public long Id { get; init; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; }
        public string? UnifiedCivilNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<ClientCard> ClientCards { get; set; }
    }
}

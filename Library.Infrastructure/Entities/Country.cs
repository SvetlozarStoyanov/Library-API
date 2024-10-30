namespace Library.Infrastructure.Entities
{
    public class Country
    {
        public Country()
        {
            Addresses = new HashSet<Address>();
            Books = new HashSet<Book>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        public long Id { get; init; }
        public string Name { get; set; }
        public string IsoCodeTwo { get; set; }
        public string IsoCodeThree { get; set; }
        public string PhoneCode { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

    }
}

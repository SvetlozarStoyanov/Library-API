namespace Library.Infrastructure.Entities
{
    public class ClientCardType
    {
        public ClientCardType()
        {
            ClientCards = new HashSet<ClientCard>();
        }

        public long Id { get; init; }
        public string Name { get; set; }
        public int CheckoutQuantityLimit { get; set; }
        public int CheckoutTimeLimit { get; set; }
        public int MonthsValid { get; set; }
        public virtual ICollection<ClientCard> ClientCards { get; set; }
    }
}

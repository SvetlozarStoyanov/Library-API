namespace Library.Core.Dto.ClientCardTypes
{
    public class ClientCardTypeListDto
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public int CheckoutTimeLimit { get; init; }
        public int CheckoutQuantityLimit { get; init; }
        public int MonthsValid { get; init; }
    }
}

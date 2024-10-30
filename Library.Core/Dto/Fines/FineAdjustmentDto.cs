namespace Library.Core.Dto.Fines
{
    public class FineAdjustmentDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }
    }
}

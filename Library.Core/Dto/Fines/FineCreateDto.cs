using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Fines
{
    public class FineCreateDto
    {
        public long CheckoutId { get; set; }
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }
        [EnumDataType(typeof(FineReason))]
        public FineReason Reason { get; set; }
        public DateTime Date { get; set; }
    }
}

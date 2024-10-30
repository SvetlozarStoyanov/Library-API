using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Checkouts
{
    public class CheckoutsFilterDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int ItemsPerPage { get; set; } = 6;
        [EnumDataType(typeof(CheckoutStatus))]
        public CheckoutStatus? Status { get; set; }
        public DateTime? CheckoutTimeMin { get; set; }
        public DateTime? CheckoutTimeMax { get; set; }
    }
}

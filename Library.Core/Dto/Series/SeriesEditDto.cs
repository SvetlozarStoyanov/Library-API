using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Series
{
    public class SeriesEditDto
    {
        public long Id { get; init; }
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Series
{
    public class SeriesCreateDto
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
    }
}

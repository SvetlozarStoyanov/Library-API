using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Genres
{
    public class GenreEditDto
    {
        public long Id { get; init; }
        [Required]
        [MinLength(2), MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(350)]
        public string Description { get; set; }
    }
}

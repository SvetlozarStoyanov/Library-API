using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Genres
{
    public class GenreCreateDto
    {
        [Required]
        [MinLength(2), MaxLength(50)]
        public string Name { get; set; }
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
    }
}

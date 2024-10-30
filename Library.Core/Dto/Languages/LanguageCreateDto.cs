using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Languages
{
    public class LanguageCreateDto
    {
        [Required]
        [RegularExpression("[a-zA-Z]{3}")]
        public string Code { get; set; }
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Name { get; set; }
    }
}

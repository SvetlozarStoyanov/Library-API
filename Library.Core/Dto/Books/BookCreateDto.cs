using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Books
{
    public class BookCreateDto
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [MinLength(10), MaxLength(18)]
        public string? ISBN { get; set; }
        [Required]
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
        public long LanguageId { get; set; }
        public long CountryId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int PageCount { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Authors
{
    public class AuthorCreateDto
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string FirstName { get; set; }
        [MinLength(2), MaxLength(100)]
        public string? MiddleName { get; set; }
        [MinLength(2), MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MinLength(5), MaxLength(350)]
        public string Description { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
    }
}

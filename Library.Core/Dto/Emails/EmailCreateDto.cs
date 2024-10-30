using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Emails
{
    public class EmailCreateDto
    {
        [Required]
        [EmailAddress]
        public string Address { get; set; }
        [EnumDataType(typeof(EmailType))]
        public EmailType Type { get; set; }
        public bool IsMain { get; set; }
    }
}

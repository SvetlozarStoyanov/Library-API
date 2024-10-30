using Library.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Emails
{
    public class EmailEditDto
    {
        public long Id { get; init; }
        [Required]
        [EmailAddress]
        public string Address { get; set; }
        [EnumDataType(typeof(EmailType))]
        public EmailType Type { get; set; }
        public bool IsMain { get; set; }
    }
}

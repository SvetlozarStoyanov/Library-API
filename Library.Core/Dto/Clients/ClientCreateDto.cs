﻿using Library.Core.Dto.Addresses;
using Library.Core.Dto.Emails;
using Library.Core.Dto.PhoneNumbers;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Dto.Clients
{
    public class ClientCreateDto
    {
        public ClientCreateDto()
        {
            Addresses = new HashSet<AddressCreateDto>();
            Emails = new HashSet<EmailCreateDto>();
            PhoneNumbers = new HashSet<PhoneNumberCreateDto>();
        }

        [Required]
        [MinLength(2), MaxLength(100)]
        public string FirstName { get; set; }
        [MinLength(2), MaxLength(100)]
        public string? MiddleName { get; set; }
        [Required]
        [MinLength(2), MaxLength(100)]
        public string LastName { get; set; }
        [StringLength(10)]
        [RegularExpression("(?<year>[0-9]{2})((?<month>(0[0-9]{1}|1[0-2]{1})|(4[1-9]|5[0-2])))(?<day>(0[1-9]{1}|1[1-9]{1}|2[0-9]{1}|3[0-1]{1}))(?<number>[0-9]{4})",
            ErrorMessage = "Invalid Unified Civil Number! Carefully check year, month and day!")]
        public string? UnifiedCivilNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<AddressCreateDto> Addresses { get; set; }
        public IEnumerable<EmailCreateDto> Emails { get; set; }
        public IEnumerable<PhoneNumberCreateDto> PhoneNumbers { get; set; }
    }
}
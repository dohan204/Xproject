using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class CreateAccountDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ProvinceId")]
        public int ProvinceId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Invalid DistrictId")]
        public int wardsCommuneId { get; set; }
    }
}

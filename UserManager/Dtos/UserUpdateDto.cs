using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Dtos
{
    public class UserUpdateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CustomId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}

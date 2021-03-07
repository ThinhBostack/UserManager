using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Dtos
{
    public class UserReadDto
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string PasswordHash { get; set; }

        public string CustomId { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Models
{
    public class User : IdentityUser
    {
        public string CustomId { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }
    }
}

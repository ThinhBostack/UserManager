using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Dtos
{
    public class UserVerifyByEmailDto
    {
        public string Email { get; set; }

        public string code { get; set; }
    }
}

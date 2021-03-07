using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Dtos
{
    public class UserVerifyByIdDto
    {
        public string Id { get; set; }

        public string code { get; set; }
    }
}

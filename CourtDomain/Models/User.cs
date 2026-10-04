using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public int? RoleId { get; set; }
    }
}

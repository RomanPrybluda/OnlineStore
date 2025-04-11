using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserUpdateResult
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int? Age { get; set; }
    }
}

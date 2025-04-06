using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.UserData.DTO
{
    public class ResultUserInfo
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }
        
        public int? Age { get; set; }
    }
}

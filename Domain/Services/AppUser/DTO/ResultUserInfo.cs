using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.AppUser.DTO
{
     public class ResultUserInfo
    {

        public string Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public List<Review> Reviews { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
    }
}

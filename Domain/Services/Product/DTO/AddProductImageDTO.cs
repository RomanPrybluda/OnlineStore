using DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AddProductImageDTO
    {
        public IFormFile MainProductImage { get; set; } = null!;
        public List<IFormFile> ProductImages { get; set; } = null!;


        public void AddProductImage(
            Product product,
            string imageBaseName,
            List<string> imageBaseNames
            )
        {
            product.MainImageBaseName = imageBaseName;
            product.ImageBaseNames = imageBaseNames;  
        }
    }
    
}

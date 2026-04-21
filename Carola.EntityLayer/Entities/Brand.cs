using System.Collections.Generic;

namespace Carola.EntityLayer.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string LogoUrl { get; set; }
        public bool Status { get; set; } 

        // Navigation: Bir markanın birden çok aracı olabilir
        public List<Car> Cars { get; set; }
    }
}
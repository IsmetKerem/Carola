using System.Collections.Generic;

namespace Carola.EntityLayer.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }   
        public string CategoryImage { get; set; }

        // Navigation
        public List<Car> Cars { get; set; }
    }
}
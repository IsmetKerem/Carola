using System;
using System.Collections.Generic;

namespace Carola.EntityLayer.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseClass { get; set; }       
        public DateTime DriverLicenseIssueDate { get; set; } 

        public string Email { get; set; }
        public string Phone { get; set; }

        public string LicenseImageUrl { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
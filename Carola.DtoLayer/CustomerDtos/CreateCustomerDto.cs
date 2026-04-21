namespace Carola.DtoLayer.CustomerDtos
{
    public class CreateCustomerDto
    {
        // OCR ile ehliyetten okunacak alanlar
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseClass { get; set; }
        public DateTime DriverLicenseIssueDate { get; set; }

        // Kullanıcının manuel gireceği alanlar (ehliyette yok)
        public string Email { get; set; }
        public string Phone { get; set; }

        // Upload edilen ehliyet görselinin path'i
        public string LicenseImageUrl { get; set; }
    }
}
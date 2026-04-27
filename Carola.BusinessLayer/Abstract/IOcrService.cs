using Carola.DtoLayer.CustomerDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IOcrService
    {
        
        Task<LicenseOcrResultDto> ExtractLicenseDataAsync(byte[] imageBytes, string mimeType);
    }
}
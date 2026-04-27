namespace Carola.DtoLayer.MailDtos
{
    public class MailMessageDto
    {
        public string ToEmail { get; set; } = "";
        public string ToName { get; set; } = "";
        public string Subject { get; set; } = "";
        public string HtmlBody { get; set; } = "";

        
        public Dictionary<string, string> EmbeddedImages { get; set; } = new();
    }
}
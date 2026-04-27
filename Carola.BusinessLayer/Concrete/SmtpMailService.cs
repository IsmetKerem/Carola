using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.MailDtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Utils;

namespace Carola.BusinessLayer.Concrete
{
    public class SmtpMailService : IMailService
    {
        private readonly ILogger<SmtpMailService> _logger;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly bool _useSsl;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly string _username;
        private readonly string _password;

        public SmtpMailService(IConfiguration configuration, ILogger<SmtpMailService> logger)
        {
            _logger = logger;
            _smtpHost  = configuration["MailSettings:SmtpHost"]  ?? "smtp.gmail.com";
            _smtpPort  = int.Parse(configuration["MailSettings:SmtpPort"] ?? "587");
            _useSsl    = bool.Parse(configuration["MailSettings:UseSsl"]  ?? "true");
            _fromEmail = configuration["MailSettings:FromEmail"] ?? throw new InvalidOperationException("FromEmail tanımlı değil");
            _fromName  = configuration["MailSettings:FromName"]  ?? "Carola";
            _username  = configuration["MailSettings:Username"]  ?? throw new InvalidOperationException("Username tanımlı değil");
            _password  = configuration["MailSettings:Password"]  ?? throw new InvalidOperationException("Password tanımlı değil");
        }

        public async Task<bool> SendAsync(MailMessageDto message)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_fromName, _fromEmail));
                mimeMessage.To.Add(new MailboxAddress(message.ToName, message.ToEmail));
                mimeMessage.Subject = message.Subject;

                var bodyBuilder = new BodyBuilder();

                
                foreach (var kv in message.EmbeddedImages)
                {
                    if (File.Exists(kv.Value))
                    {
                        var image = bodyBuilder.LinkedResources.Add(kv.Value);
                        image.ContentId = kv.Key;
                        image.ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Inline);
                    }
                    else
                    {
                        _logger.LogWarning("Embedded image bulunamadı: {Path}", kv.Value);
                    }
                }

                bodyBuilder.HtmlBody = message.HtmlBody;
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpHost, _smtpPort,
                    _useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mimeMessage);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Mail başarıyla gönderildi: {Email}", message.ToEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mail gönderimi başarısız: {Email}", message.ToEmail);
                return false;
            }
        }
    }
}
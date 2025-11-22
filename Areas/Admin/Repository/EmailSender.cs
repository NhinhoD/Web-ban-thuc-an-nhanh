using System.Net.Mail;
using System.Net;

namespace ASM_C_4.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration vào Constructor
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // 1. Đọc thông tin từ cấu hình (appsettings.json / Secrets / Environment Variables)
            var mailHost = "smtp.gmail.com";
            var mailPort = 587;
            var mailEmail = _configuration["EmailSettings:Email"]; // Đọc email
            var mailPassword = _configuration["EmailSettings:Password"]; // Đọc pass

            // 2. Khởi tạo client
            var client = new SmtpClient(mailHost, mailPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailEmail, mailPassword)
            };

            // 3. Gửi email
            // Lưu ý: Email người gửi (from) PHẢI GIỐNG với email đăng nhập (mailEmail)
            // Nếu khác nhau, Gmail sẽ chặn hoặc đánh dấu là Spam.
            return client.SendMailAsync(
                new MailMessage(from: mailEmail,
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
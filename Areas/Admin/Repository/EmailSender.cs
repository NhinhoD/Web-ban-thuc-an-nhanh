using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ASM_C_4.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // 1. Đọc cấu hình
            var myEmail = "duongpxps38124@gmail.com";
            var myPassword = _configuration["EmailSettings:Password"]; // Đọc từ Render Environment

            // 2. Tạo nội dung email bằng MimeKit (Chuẩn mới)
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("FastFood Shop", myEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            // Nội dung HTML
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // 3. Gửi email bằng MailKit (SmtpClient của MailKit xịn hơn System.Net.Mail)
            using (var client = new SmtpClient())
            {
                // Kết nối đến Gmail port 587 (STARTTLS)
                // MailKit sẽ tự động thử IPv4 nếu IPv6 lỗi -> Khắc phục được lỗi "Network Unreachable"
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Đăng nhập
                await client.AuthenticateAsync(myEmail, myPassword);

                // Gửi
                await client.SendAsync(emailMessage);

                // Ngắt kết nối
                await client.DisconnectAsync(true);
            }
        }
    }
}
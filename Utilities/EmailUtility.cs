using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;
using MAVE.DTO;

namespace MAVE.Utilities
{
    public class EmailUtility : IEmailUtility
    {
        private readonly IConfiguration _config;
        public EmailUtility(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDTO request)
        {
            var userName = _config["Email:UserName"];
            var passWord = _config["Email:PassWord"];
            var host = _config["Email:Host"];
            var portText = _config["Email:Port"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portText))
            {
                throw new InvalidOperationException("Faltan los datos de configuración del correo.");
            }
            if (!int.TryParse(portText, out var port))
            {
                throw new InvalidOperationException("El puerto del correo no es un número válido.");
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(userName));

            // Separar las direcciones de correo destinatarias por comas y agregarlas al campo "To"
            foreach (var destinatario in request.Addressee.Split(','))
            {
                email.To.Add(MailboxAddress.Parse(destinatario.Trim()));
            }

            email.Subject = request.Affair;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Contain,
            };

            using var smtp = new SmtpClient();
            smtp.Connect(host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(userName, passWord);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
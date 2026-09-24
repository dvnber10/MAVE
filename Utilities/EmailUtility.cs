using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;
using MAVE.DTO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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
            // Tolera únicamente "revocación desconocida" (redes que bloquean
            // la comprobación OCSP/CRL). Cualquier otro error de cadena sigue
            // rechazándose para no aceptar certificados falsos.
            smtp.ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None) return true;
                if (errors != SslPolicyErrors.RemoteCertificateChainErrors || chain == null) return false;
                foreach (var status in chain.ChainStatus)
                {
                    if (status.Status != X509ChainStatusFlags.RevocationStatusUnknown &&
                        status.Status != X509ChainStatusFlags.OfflineRevocation)
                        return false;
                }
                return true;
            };
            smtp.Connect(host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(userName, passWord);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
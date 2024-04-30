using MAVE.DTO;
using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;

namespace MAVE.Services
{
    public class UserService
    {
        // acess to utilities in oter folders  
        private readonly UserRepositories _repo;
        private readonly TokenAndEncipt _tk;
        private readonly EmailUtility _mail;
        public UserService(UserRepositories repo, TokenAndEncipt token, EmailUtility mail)
        {
            _mail= mail;
            _repo = repo;
            _tk = token;
        }

        //Delete users method
        public async Task<bool> UserDelete(int? id)
        {
            var userDelete = await _repo.GetUserByID(id);
            if (id == null || userDelete == null) //verify id or user not null
            {
                return false;
            }
            if (userDelete != null)
            {
                await _repo.DeleteUser(userDelete); // delete user of the database 
            }
            return true;
        }

        public async Task<User> GetUserByMail(string email){
#pragma warning disable CS8603 // Possible null reference return.
            return await _repo.GetUserByMail(email);
#pragma warning restore CS8603 // Possible null reference return.
        }

        //Update users method
        public async Task<bool> UpdateUser(UserSigInDTO user)
        {
            var userI = _repo.GetUserByMail(user.Email);
            if (userI == null) return false;
            // modify user for add to database
            var userU = new User{
                    //Name = user.UserName,
                    Phone = user.Phone,
                    Password = user.Password
                };
            user.Password = TokenAndEncipt.HashPass(userU.Password);
            await _repo.UpdateUser(userU);
            return true;
        }

        //Create Users Method
        public async Task<bool> CreateUser(UserSigInDTO user)
        {
            //verify entry not null
            if(await _repo.GetUserByMail(user.Email) == null)
            {
                String url = "https://bvdnxbgz-5173.use2.devtunnels.ms";
                var userU = new User{
                    Email = user.Email,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Password = user.Password,
                    RoleId = 4,
                    EvaluationId = 1,
                    StatusId = 1
                };
                userU.Password = TokenAndEncipt.HashPass(user.Password); // encript password for bcryp
                await _repo.CreateUser(userU); //save changes in database
                // create email config for send confirmation 
                var emailRequest = new EmailDTO{
                    Addressee = user.Email,
                    Affair = "Register Mave",
                    Contain = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html dir=\"ltr\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head><meta charset=\"UTF-8\"><meta content=\"width=device-width, initial-scale=1\" name=\"viewport\"><meta name=\"x-apple-disable-message-reformatting\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta content=\"telephone=no\" name=\"format-detection\"><title></title><!--[if (mso 16)]><style type=\"text/css\">a {text-decoration: none;}</style><![endif]--><!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]--><!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG></o:AllowPNG><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]--><!--[if mso]><style type=\"text/css\">ul {margin: 0 !important;}ol {margin: 0 !important;}li {margin-left: 47px !important;}</style><![endif]--></head><body class=\"body\"><div dir=\"ltr\" class=\"es-wrapper-color\"><!--[if gte mso 9]><v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\"><v:fill type=\"tile\" color=\"#f6f6f6\"></v:fill></v:background><![endif]--><table class=\"es-wrapper\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-email-paddings\" valign=\"top\"><table class=\"esd-header-popover es-header\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-header-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"es-p20t es-p20r es-p20l esd-structure\" align=\"left\" bgcolor=\"#1b5091\" style=\"background-color:#1b5091\"><!--[if mso]><table width=\"560\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"246\" valign=\"top\"><![endif]--><table class=\"es-left\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tbody><tr><td width=\"246\" class=\"esd-container-frame es-m-p20b\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tbody><tr><td align=\"center\" class=\"esd-block-image\" style=\"font-size: 0\"><a target=\"_blank\"><img src=\"https://fiepcgl.stripocdn.email/content/guids/CABINET_28007b800008bc750ac791e848023f4fab0f12f58fa0f1925c5e5067b22cf37f/images/logo_1.png\" alt=\"\" width=\"246\"></a></td></tr></tbody></table></td></tr></tbody></table><!--[if mso]></td><td width=\"20\"></td><td width=\"294\" valign=\"top\"><![endif]--><table class=\"es-right\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\"><tbody><tr><td class=\"esd-container-frame\" width=\"294\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"left\" class=\"esd-block-text\"><p></p></td></tr><tr><td align=\"left\" class=\"esd-block-text es-text-7400\"><h1 style=\"color:#ffffff;font-family:tahoma,verdana,segoe,sans-serif\"><strong style=\"font-size:72px;line-height:150%\">MAVE</strong></h1><h1 style=\"color:#ffffff\"><strong style=\"font-size:24px;line-height:150%\">Mente en Armonia, &nbsp;</strong></h1><h1 style=\"color:#ffffff\"><strong><span style=\"font-size:24px;line-height:150%\" class=\"es-text-mobile-size-24\">Vida en Equilibrio.</span><span style=\"font-size:36px;line-height:150%\"></span><span class=\"es-text-mobile-size-36\"></span></strong></h1></td></tr></tbody></table></td></tr></tbody></table><!--[if mso]></td></tr></table><![endif]--></td></tr></tbody></table></td></tr></tbody></table><table class=\"es-content\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-content-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"es-p20t es-p20r es-p20l esd-structure\" align=\"left\" bgcolor=\"#6ea1d4\" style=\"background-color:#6ea1d4\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-container-frame\" width=\"560\" valign=\"top\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"center\" class=\"esd-block-spacer es-p20\" style=\"font-size: 0\"><table border=\"0\" width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" class=\"es-spacer\"><tbody><tr><td style=\"border-bottom: 1px solid #cccccc;; background: none; height: 1px; width: 100%; margin: 0px 0px 0px 0px\"></td></tr></tbody></table></td></tr><tr><td align=\"left\" class=\"esd-block-text\"><h2 align=\"center\" style=\"color:#1b5091;font-family:tahoma,verdana,segoe,sans-serif\"><strong>Registro exitoso, Bienvenido a MAVE.</strong></h2></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table><table class=\"esd-footer-popover es-footer\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-footer-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"esd-structure es-p20t es-p20b es-p20r es-p20l\" align=\"left\" bgcolor=\"#6ea1d4\" style=\"background-color:#6ea1d4\"><table class=\"es-right\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\"><tbody><tr><td class=\"esd-container-frame\" width=\"560\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"center\" class=\"esd-block-button\"><!--[if mso]><a href="+url+" target=\"_blank\" hidden><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" esdevVmlButton href="+url+" style=\"height:49px; v-text-anchor:middle; width:189px\" arcsize=\"50%\" strokecolor=\"#ce3375\" strokeweight=\"2px\" fillcolor=\"#1b5091\"><w:anchorlock></w:anchorlock><center style='color:#ffffff; font-family:tahoma, verdana, segoe, sans-serif; font-size:20px; font-weight:700; line-height:20px;  mso-text-raise:1px'>Ir a MAVE</center></v:roundrect></a><![endif]--><!--[if !mso]><!-- --><span class=\"es-button-border\" style=\"background:#1b5091;border-color:#CE3375\"><a href="+url+" class=\"es-button\" target=\"_blank\" style=\"background:#1b5091;mso-border-alt:10px solid #1b5091;font-weight:bold;font-size:26px;font-family:tahoma,verdana,segoe,sans-serif\">Ir a MAVE</a></span><!--<![endif]--></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody>" 
                }; 
                _mail.SendEmail(emailRequest);
                return true;   
            }
            else
            {
                return false;
            }
        }

        //Login Method
        public async Task<int> LogIn(string user, string pass)
        {
            var UserAct = await _repo.GetUserByMail(user);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var password = UserAct.Password;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (UserAct == null)
            {
                return 0;
            }
            else if (BCrypt.Net.BCrypt.Verify(pass, password))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public async Task<int> RecoveryPass (string mail){
            var user = await _repo.GetUserByMail(mail);
            if (user ==null)
            {
                return 0;
            }
            else
            {
                var tokenPass = _tk.GenerarToken(mail,Convert.ToString(user.UserId));
                string url = "https://v00lqp9l-5173.use2.devtunnels.ms/ResetPassword/?token="+tokenPass+"/?id="+user.UserId;
                var emailRequest = new EmailDTO{
                    Addressee = user.Email,
                    Affair = "Recovery Password Mave",
                    Contain = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html dir=\"ltr\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head><meta charset=\"UTF-8\"><meta content=\"width=device-width, initial-scale=1\" name=\"viewport\"><meta name=\"x-apple-disable-message-reformatting\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta content=\"telephone=no\" name=\"format-detection\"><title></title><!--[if (mso 16)]><style type=\"text/css\">a {text-decoration: none;}</style><![endif]--><!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]--><!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG></o:AllowPNG><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]--><!--[if mso]><style type=\"text/css\">ul {margin: 0 !important;}ol {margin: 0 !important;}li {margin-left: 47px !important;}</style><![endif]--></head><body class=\"body\"><div dir=\"ltr\" class=\"es-wrapper-color\"><!--[if gte mso 9]><v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\"><v:fill type=\"tile\" color=\"#f6f6f6\"></v:fill></v:background><![endif]--><table class=\"es-wrapper\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-email-paddings\" valign=\"top\"><table class=\"esd-header-popover es-header\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-header-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"es-p20t es-p20r es-p20l esd-structure\" align=\"left\" bgcolor=\"#1b5091\" style=\"background-color:#1b5091\"><!--[if mso]><table width=\"560\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"246\" valign=\"top\"><![endif]--><table class=\"es-left\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tbody><tr><td width=\"246\" class=\"esd-container-frame es-m-p20b\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tbody><tr><td align=\"center\" class=\"esd-block-image\" style=\"font-size: 0\"><a target=\"_blank\"><img src=\"https://fiepcgl.stripocdn.email/content/guids/CABINET_28007b800008bc750ac791e848023f4fab0f12f58fa0f1925c5e5067b22cf37f/images/logo_1.png\" alt=\"\" width=\"246\"></a></td></tr></tbody></table></td></tr></tbody></table><!--[if mso]></td><td width=\"20\"></td><td width=\"294\" valign=\"top\"><![endif]--><table class=\"es-right\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\"><tbody><tr><td class=\"esd-container-frame\" width=\"294\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"left\" class=\"esd-block-text\"><p></p></td></tr><tr><td align=\"left\" class=\"esd-block-text es-text-7400\"><h1 style=\"color:#ffffff;font-family:tahoma,verdana,segoe,sans-serif\"><strong style=\"font-size:72px;line-height:150%\">MAVE</strong></h1><h1 style=\"color:#ffffff\"><strong style=\"font-size:24px;line-height:150%\">Mente en Armonia, &nbsp;</strong></h1><h1 style=\"color:#ffffff\"><strong><span style=\"font-size:24px;line-height:150%\" class=\"es-text-mobile-size-24\">Vida en Equilibrio.</span><span style=\"font-size:36px;line-height:150%\"></span><span class=\"es-text-mobile-size-36\"></span></strong></h1></td></tr></tbody></table></td></tr></tbody></table><!--[if mso]></td></tr></table><![endif]--></td></tr></tbody></table></td></tr></tbody></table><table class=\"es-content\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-content-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"es-p20t es-p20r es-p20l esd-structure\" align=\"left\" bgcolor=\"#6ea1d4\" style=\"background-color:#6ea1d4\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-container-frame\" width=\"560\" valign=\"top\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"center\" class=\"esd-block-spacer es-p20\" style=\"font-size: 0\"><table border=\"0\" width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" class=\"es-spacer\"><tbody><tr><td style=\"border-bottom: 1px solid #cccccc;; background: none; height: 1px; width: 100%; margin: 0px 0px 0px 0px\"></td></tr></tbody></table></td></tr><tr><td align=\"left\" class=\"esd-block-text\"><h2 align=\"center\" style=\"color:#1b5091;font-family:tahoma,verdana,segoe,sans-serif\"><strong>Parece que quieres recuperar tu Contraseña, si es así da clic en el botón \"Recuperar\", si no ignora este correo.</strong></h2></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table><table class=\"esd-footer-popover es-footer\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-footer-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"esd-structure es-p20t es-p20b es-p20r es-p20l\" align=\"left\" bgcolor=\"#6ea1d4\" style=\"background-color:#6ea1d4\"><table class=\"es-right\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\"><tbody><tr><td class=\"esd-container-frame\" width=\"560\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td align=\"center\" class=\"esd-block-button\"><!--[if mso]><a href="+url+" target=\"_blank\" hidden><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" esdevVmlButton href="+url+" style=\"height:49px; v-text-anchor:middle; width:189px\" arcsize=\"50%\" strokecolor=\"#ce3375\" strokeweight=\"2px\" fillcolor=\"#1b5091\"><w:anchorlock></w:anchorlock><center style='color:#ffffff; font-family:tahoma, verdana, segoe, sans-serif; font-size:20px; font-weight:700; line-height:20px;  mso-text-raise:1px'>Recuperar</center></v:roundrect></a><![endif]--><!--[if !mso]><!-- --><span class=\"es-button-border\" style=\"background:#1b5091;border-color:#CE3375\"><a href="+url+" class=\"es-button\" target=\"_blank\" style=\"background:#1b5091;mso-border-alt:10px solid #1b5091;font-weight:bold;font-size:26px;font-family:tahoma,verdana,segoe,sans-serif\">Recuperar</a></span><!--<![endif]--></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody>" 
                };
                _mail.SendEmail(emailRequest);
                return 1;
            }
        }
        public async Task<int> ResetPass(int? id, string pass ){
            var userA=await _repo.GetUserByID(id);
            userA.Password = TokenAndEncipt.HashPass(pass);
            await _repo.UpdateUser(userA);
            return 1;
        }
        public async Task<User> GetUserById(int? id){
            return await _repo.GetUserByID(id);
        }
    }
}

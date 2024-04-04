using MAVE.DTO;
using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MAVE.Services
{
    public class UserService
    {
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
            if (id == null || userDelete == null)
            {
                return false;
            }
            if (userDelete != null)
            {
                await _repo.DeleteUser(userDelete);
            }
            return true;
        }

        //Update users method
        public async Task<bool> UpdateUser(UserSigInDTO user)
        {
            var userU = new User{
                    UserId=1,
                    Name = user.UserName,
                    Phone = Convert.ToString(user.Phone),
                    Password = user.Password,
                    RoleId = 1,
                    EvaluationId = 1
                };
            
            var userI = _repo.GetUserByID(userU.UserId);
            if (userI == null) return false;
            user.Password = TokenAndEncipt.HashPass(userU.Password);
            await _repo.UpdateUser(userU);
            return true;
        }

        //Create Users Method
        public async Task<bool> CreateUser(UserSigInDTO user)
        {
            //verify entry not null
            if(_repo.GetUserByMail(user.Email) == null)
            {
                return false;
            }
            else
            {
                //modify user for export to database
                var userU = new User{
                    Email = user.Email,
                    UserName = user.UserName,
                    Phone = Convert.ToString(user.Phone),
                    Password = user.Password,
                    RoleId = 2,
                    EvaluationId = 1
                };
                userU.Password = TokenAndEncipt.HashPass(user.Password); // encript password for bcryp
                await _repo.CreateUser(userU); //save changes in database
                // create email config for send confirmation 
                var emailRequest = new EmailDTO{
                    Addressee = user.Email,
                    Affair = "Register Mave",
                    Contain = "Welcome to Mave" 
                }; 
                _mail.SendEmail(emailRequest);
                return true;
            }
        }

        //Login Method
        public async Task<int> LogIn(string user, string pass)
        {
            var UserAct = await _repo.GetUserByMail(user);
            var password = UserAct.Password;
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
                var tokenPass = _tk.GenerarToken(mail);
                string url = "http://localhost:5173/ResetPass/?token"+tokenPass;
                var emailRequest = new EmailDTO{
                    Addressee = user.Email,
                    Affair = "Recovery Password Mave",
                    Contain = "<p>Correo para recuperacion de contraseña</p>"+"<a href='"+url+"'> Click para recuperar </a>" 
                };
                _mail.SendEmail(emailRequest);
                return 1;
            }
        }
        public async Task<int> ResetPass(string email, string pass){
            var userA=await _repo.GetUserByMail(email);
            userA.Password = TokenAndEncipt.HashPass(pass);
            await _repo.UpdateUser(userA);
            return 1;
        }
    }
}

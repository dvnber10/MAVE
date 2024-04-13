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
                    Name = user.UserName,
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
                //modify user for export to database
                var userU = new User{
                    Email = user.Email,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Password = user.Password,
                    RoleId = 2,
                    EvaluationId = 1,
                    StatusId = 1
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
                    Contain = "<p>Correo para recuperacion de contraseña</p>"+"<a href='"+url+"'> Click para recuperar </a>" 
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

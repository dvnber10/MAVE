using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace MAVE.Services
{
    public class UserService
    {
        private readonly UserRepositories _repo;
        private readonly TokenAndEncipt _token;
        public UserService(UserRepositories repo)
        {
            _repo = repo;
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
        public async Task<bool> UpdateUser([FromForm] UserModel user)
        {
            var userU = _repo.GetUserByID(user.Id);
            if (userU == null) return false;
            user.Pass = TokenAndEncipt.HashPass(user.Pass);
            await _repo.UpdateUser(user);
            return true;
        }

        //Create Users Method
        public async Task<bool> CreateUser([FromForm] UserModel user)
        {
            if(_repo.GetUserByMail(user.Email) != null)
            {
                return false;
            }
            else
            {
                user.Pass = TokenAndEncipt.HashPass(user.Pass);
                await _repo.CreateUser(user);
                return true;
            }
        }

        //Login Method
        public async Task<int> LogIn([FromBody] UserModel user)
        {
            var UserAct = await _repo.GetUserByMail(user.Email);
            var password = UserAct.Pass;
            if (UserAct == null)
            {
                return 0;
            }
            else if (BCrypt.Net.BCrypt.Verify(user.Pass, password))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

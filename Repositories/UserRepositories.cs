using MAVE.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace MAVE.Repositories
{
    public class UserRepositories
    {
        private readonly  DbAa60a4MavetestContext _context;
        public UserRepositories(DbAa60a4MavetestContext context){
            _context = context;
        }
        public async Task UpdateUser (UserModel user){
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser (UserModel user){
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task CreateUser(UserModel user){
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserModel>> GetAllUsers (){
            
            return await _context.Users.ToListAsync();
        }
        public async Task<UserModel> GetUserByID(int? id){
            var user = await _context.Users.FindAsync(id);
            #pragma warning disable CS8603 // Possible null reference return.
            return user;
            #pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<UserModel> GetUserByMail(string mail){
            var userC = await _context.Users.Where(c => c.Email ==mail).FirstOrDefaultAsync();
            #pragma warning disable CS8603 // Possible null reference return.
            return userC;
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
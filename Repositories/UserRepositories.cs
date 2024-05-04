using System.Numerics;
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
        public async Task UpdateUser (User user){
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser (User user){
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task CreateUser(User user){
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsers (){
            
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserByID(int? id){
            #nullable disable
            var user = await _context.Users.FindAsync(id);
            user.Status = await _context.CatStatuses.Where(cs => cs.StatusId == user.StatusId).FirstOrDefaultAsync();
            user.Role = await _context.CatRoles.Where(cr => cr.RoleId == user.RoleId).FirstOrDefaultAsync();
            return user;
            #nullable enable
        }
        public async Task<User?> GetUserByMail(string mail){
            var userC = await _context.Users.FirstOrDefaultAsync(e=>e.Email==mail);
            return userC;
        }
    }
}

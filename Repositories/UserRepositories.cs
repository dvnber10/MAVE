using System.Numerics;
using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{
    public class UserRepositories
    {
        private readonly  DbAa60a4MavetestContext _context;
        public UserRepositories(DbAa60a4MavetestContext context){
            _context = context;
        }
        public async Task<List<User>> GetAllUser(){
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task UpdateUserComplete (User user,UpdateUserDTO userEntrante){
            user.RoleId = userEntrante.IdRole;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task CreatePsychologistProfile(PsychologistProfile profile){
            await _context.PsychologistProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }
        public async Task DeletePsychologistProfile(int userId){
            var p = await _context.PsychologistProfiles.FindAsync(userId);
            if (p != null)
            {
                _context.PsychologistProfiles.Remove(p);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<User>> GetAllUsers (){
            
            return await _context.Users.ToListAsync();
        }
        public async Task<List<User>> GetPsychologists (){
            
            return await _context.Users
                .Where(u => u.RoleId == 3 && u.StatusId == 1)
                .ToListAsync();
        }
        public async Task<List<User>> GetPatientsByProfessional (int professionalId){
            
            return await _context.Users
                .Where(u => u.HealthProfessionalId == professionalId)
                .ToListAsync();
        }
        public async Task<List<MAVE.DTO.ProfessionalDTO>> GetProfessionalDirectory (){
            
            return await (from u in _context.Users
                          join p in _context.PsychologistProfiles on u.UserId equals p.UserId into pj
                          from p in pj.DefaultIfEmpty()
                          where u.RoleId == 3 && u.StatusId == 1
                          select new MAVE.DTO.ProfessionalDTO{
                              UserId = u.UserId,
                              UserName = u.UserName,
                              Email = u.Email,
                              Phone = u.Phone,
                              Description = p != null ? p.Description : "",
                              Verified = p != null && p.Verified
                          }).ToListAsync();
        }
        public async Task<List<MAVE.DTO.PendingPsyDTO>> GetPendingPsychologists (){
            return await (from u in _context.Users
                          join p in _context.PsychologistProfiles on u.UserId equals p.UserId into pj
                          from p in pj.DefaultIfEmpty()
                          where u.RoleId == 3 && (p == null || !p.Verified)
                          orderby u.UserId descending
                          select new MAVE.DTO.PendingPsyDTO{
                              UserId = u.UserId,
                              UserName = u.UserName,
                              Email = u.Email,
                              Phone = u.Phone,
                              Description = p != null ? p.Description : "",
                              CredentialUrl = p != null && p.CredentialUrl != null ? p.CredentialUrl : "",
                              Verified = false
                          }).ToListAsync();
        }
        public async Task<User> GetUserByID(int? id){
            #nullable disable
            var user = await _context.Users.FindAsync(id);
            user.Status = await _context.CatStatuses.Where(cs => cs.StatusId == user.StatusId).FirstOrDefaultAsync();
            user.Role = await _context.CatRoles.Where(cr => cr.RoleId == user.RoleId).FirstOrDefaultAsync();
            return user;
            #nullable enable
        }
        public async Task<User> GetUserByIdFromInfo(int? id){
            var user = await _context.Users.FindAsync(id);
#pragma warning disable CS8603 // Possible null reference return.
            return user;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<User?> GetUserByMail(string mail){
            var userC = await _context.Users.FirstOrDefaultAsync(e=>e.Email==mail);
            return userC;
        }

        public async Task<User?> GetUserByName(string name)
        {
            var user = await _context.Users.Where(u => u.UserName==name).FirstOrDefaultAsync();
            return user;
        }
    }
}

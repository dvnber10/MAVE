using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using MAVE.DTO;
using MAVE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Services
{
    /// <summary>
    /// Perfil de psicólogo: descripción, documento credencial (PDF/imagen a
    /// Cloudinary) y verificación por admin. Sin migraciones de USER.
    /// </summary>
    public class PsychologistService
    {
        private readonly DbAa60a4MavetestContext _db;

        public PsychologistService(DbAa60a4MavetestContext db)
        {
            _db = db;
        }

        private async Task<User?> CallerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        private static bool IsAdmin(User u) => u.RoleId == 1 || u.RoleId == 2;

        private static bool CanEdit(User caller, int userId) =>
            caller.UserId == userId || IsAdmin(caller);

        public async Task<PsychologistProfileDTO?> GetProfile(int userId)
        {
            var p = await _db.PsychologistProfiles.FindAsync(userId);
            if (p == null) return new PsychologistProfileDTO();
            return new PsychologistProfileDTO
            {
                Description = p.Description,
                CredentialUrl = p.CredentialUrl ?? string.Empty,
                Verified = p.Verified
            };
        }

        public async Task<bool> SaveDescription(string callerEmail, int userId, string description)
        {
            var caller = await CallerByEmail(callerEmail);
            if (caller == null || !CanEdit(caller, userId)) return false;
            var target = await _db.Users.FindAsync(userId);
            if (target == null || target.RoleId != 3) return false;
            description = (description ?? string.Empty).Trim();
            if (description.Length > 500) return false;
            var p = await _db.PsychologistProfiles.FindAsync(userId);
            if (p == null)
            {
                p = new PsychologistProfile { UserId = userId, Verified = false, UpdatedAt = DateTime.Now };
                _db.PsychologistProfiles.Add(p);
            }
            p.Description = description;
            p.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Ok, string Message)> UploadCredential(string callerEmail, int userId, IFormFile? file)
        {
            var caller = await CallerByEmail(callerEmail);
            if (caller == null || !CanEdit(caller, userId)) return (false, "No autorizado");
            var target = await _db.Users.FindAsync(userId);
            if (target == null || target.RoleId != 3) return (false, "Solo psicólogos");
            if (file == null || file.Length == 0) return (false, "Archivo vacío");
            if (file.Length > 10 * 1024 * 1024) return (false, "Máximo 10MB");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".pdf" && ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                return (false, "Solo PDF, JPG o PNG");
            string tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ext);
            try
            {
                await using (var stream = File.Create(tmp))
                    await file.CopyToAsync(stream);
                DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
                Cloudinary cloudinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
                cloudinary.Api.Secure = true;
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(tmp),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };
                var result = cloudinary.Upload(uploadParams);
                string url = Convert.ToString(result.SecureUrl) ?? string.Empty;
                if (string.IsNullOrEmpty(url)) return (false, "Falló la subida");
                var p = await _db.PsychologistProfiles.FindAsync(userId);
                if (p == null)
                {
                    p = new PsychologistProfile { UserId = userId, Verified = false, UpdatedAt = DateTime.Now };
                    _db.PsychologistProfiles.Add(p);
                }
                p.CredentialUrl = url;
                p.Verified = false;
                p.UpdatedAt = DateTime.Now;
                await _db.SaveChangesAsync();
                return (true, "Documento cargado, pendiente de verificación");
            }
            catch (Exception e)
            {
                return (false, "No se pudo subir: " + e.Message);
            }
            finally
            {
                try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
            }
        }

        public async Task<bool> Verify(string callerEmail, int userId, bool verified)
        {
            var caller = await CallerByEmail(callerEmail);
            if (caller == null || !IsAdmin(caller)) return false;
            var p = await _db.PsychologistProfiles.FindAsync(userId);
            if (p == null) return false;
            p.Verified = verified;
            p.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Services
{
    /// <summary>
    /// Chat interno paciente-psicólogo. La autorización se verifica por email
    /// del JWT (el claim Role trae el UserId, no sirve para roles).
    /// </summary>
    public class ChatService
    {
        private readonly DbAa60a4MavetestContext _db;

        public ChatService(DbAa60a4MavetestContext db)
        {
            _db = db;
        }

        private async Task<User?> Caller(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        private static bool IsAdmin(User u) => u.RoleId == 1 || u.RoleId == 2;

        private static bool CanSee(User caller, int userId) =>
            caller.UserId == userId || IsAdmin(caller);

        public async Task<List<ConversationDTO>?> Conversations(string callerEmail, int userId)
        {
            var caller = await Caller(callerEmail);
            if (caller == null || !CanSee(caller, userId)) return null;
            var msgs = await _db.ChatMessages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.Date)
                .Take(200)
                .ToListAsync();
            var peerIds = msgs.Select(m => m.SenderId == userId ? m.ReceiverId : m.SenderId).Distinct().ToList();
            var peers = await _db.Users.Where(u => peerIds.Contains(u.UserId)).ToDictionaryAsync(u => u.UserId);
            var list = new List<ConversationDTO>();
            foreach (var pid in peerIds)
            {
                if (!peers.TryGetValue(pid, out var p)) continue;
                var pm = msgs.Where(m => m.SenderId == pid || m.ReceiverId == pid).ToList();
                var last = pm.OrderByDescending(m => m.Date).First();
                list.Add(new ConversationDTO
                {
                    UserId = pid,
                    UserName = p.UserName,
                    LastText = last.Text,
                    LastDate = last.Date,
                    Unread = pm.Count(m => m.ReceiverId == userId && !m.Read)
                });
            }
            return list.OrderByDescending(c => c.LastDate).ToList();
        }

        public async Task<List<ChatMessageDTO>?> History(string callerEmail, int userId, int otherId)
        {
            var caller = await Caller(callerEmail);
            if (caller == null || !CanSee(caller, userId)) return null;
            var msgs = await _db.ChatMessages
                .Where(m => (m.SenderId == userId && m.ReceiverId == otherId) ||
                            (m.SenderId == otherId && m.ReceiverId == userId))
                .OrderBy(m => m.Date)
                .Take(200)
                .ToListAsync();
            var names = await _db.Users
                .Where(u => u.UserId == userId || u.UserId == otherId)
                .ToDictionaryAsync(u => u.UserId, u => u.UserName);
            foreach (var m in msgs.Where(m => m.ReceiverId == userId && !m.Read))
                m.Read = true;
            await _db.SaveChangesAsync();
            return msgs.Select(m => new ChatMessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Text = m.Text,
                Date = m.Date,
                Read = m.Read,
                SenderName = names.TryGetValue(m.SenderId, out var n) ? n : ""
            }).ToList();
        }

        public async Task<ChatMessageDTO?> Send(string callerEmail, int senderId, int receiverId, string text)
        {
            var caller = await Caller(callerEmail);
            if (caller == null || caller.UserId != senderId) return null;
            text = (text ?? string.Empty).Trim();
            if (text.Length == 0 || text.Length > 500) return null;
            var receiver = await _db.Users.FindAsync(receiverId);
            if (receiver == null) return null;
            var m = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = text,
                Date = DateTime.Now,
                Read = false
            };
            _db.ChatMessages.Add(m);
            await _db.SaveChangesAsync();
            return new ChatMessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Text = m.Text,
                Date = m.Date,
                Read = false,
                SenderName = caller.UserName
            };
        }
    }
}

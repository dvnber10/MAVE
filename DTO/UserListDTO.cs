namespace MAVE.DTO
{
    /// <summary>Lista de usuarios para administración (plano, sin ciclos de navegación).</summary>
    public class UserListDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public short StatusId { get; set; }
        public int? HealthProfessionalId { get; set; }
    }
}

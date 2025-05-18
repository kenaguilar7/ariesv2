namespace Aries.Contabilidad.Models.Auth
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Usuario = 1,
        Administrador = 2
    }
} 
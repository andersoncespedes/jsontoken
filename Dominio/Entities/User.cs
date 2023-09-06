

namespace Dominio.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public ICollection<Rol> Rols{ get; set; } = new HashSet<Rol>();
    public ICollection<UserRol> UsersRols{ get; set; }



}

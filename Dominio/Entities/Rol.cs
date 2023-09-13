
namespace Dominio.Entities;

public class Rol : BaseEntity
{
    public string Nombre { get; set; }
    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public ICollection<UserRols> UsersRoles{ get; set; }
}

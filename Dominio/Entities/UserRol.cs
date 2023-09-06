
namespace Dominio.Entities;

public class UserRol : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int RolId { get; set; }
    public Rol Rol { get; set; }
}

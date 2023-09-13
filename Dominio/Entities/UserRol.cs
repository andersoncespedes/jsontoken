
namespace Dominio.Entities;

public class UserRols : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int RolId { get; set; }
    public Rol Rol { get; set; }
}

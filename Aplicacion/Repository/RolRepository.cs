using System.Linq.Expressions;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia.Data;

namespace Aplicacion.Repository;

public class RolRepository : GenericRepository<Rol>, IRol
{
    private readonly APIContext _context;
    public RolRepository(APIContext context) : base(context)
    {
        _context = context;
    }
    public override void Add(Rol entity)
    {
        base.Add(entity);
    }
    public override IEnumerable<Rol> Find(Expression<Func<Rol, bool>> expression)
    {
        return base.Find(expression);
    }
    public override void Update(Rol entities)
    {
        base.Update(entities);
    }

}

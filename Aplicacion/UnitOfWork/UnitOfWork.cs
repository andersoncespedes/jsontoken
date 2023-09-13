using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Repository;
using Dominio.Interfaces;
using Persistencia.Data;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly APIContext _context;
    private UserRepository _Users;
    private RolRepository _Rolses;
    public UnitOfWork(APIContext context){
        _context = context;
    }
    public IRol Roles {
        get{
            if(_Rolses == null){
               _Rolses = new(_context); 
            }
            return _Rolses;
        }
    }

    public IUser User {
        get {
            if(_Users ==null) _Users = new UserRepository(_context);
            return _Users;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

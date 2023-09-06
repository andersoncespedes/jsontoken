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
    private UsuarioRepository _usuarios;
    private RolRepository _roles;


    public UnitOfWork(APIContext context){
        _context = context;
    }
    public IRol Rol {
        get{
            if(_roles == null){
               _roles = new(_context); 
            }
            return _roles;
        }
    }

    public IUsuario Usuario {
        get {
            if(_usuarios ==null) _usuarios = new UsuarioRepository(_context);
            return _usuarios;
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

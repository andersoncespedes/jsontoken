using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Interfaces;

public interface IUnitOfWork
{
    IRol Rol {get;}
    IUsuario Usuario {get;} 
    Task<int> SaveAsync();
}

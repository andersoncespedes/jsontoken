using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Interfaces;

public interface IUnitOfWork
{
    IRol Roles {get;}
    IUser User {get;} 
    Task<int> SaveAsync();
}

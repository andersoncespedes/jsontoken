using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
namespace Aplicacion.Repository
{
    public class UsuarioRepository : GenericRepository<User>, IUsuario
    {
        private readonly APIContext _context;
        public UsuarioRepository(APIContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<User>> GetAllAsync(){
            return await base.GetAllAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(e => e.Rols)
            .FirstOrDefaultAsync(e => e.Username.ToLower() == username.ToLower());
        }
    }
}
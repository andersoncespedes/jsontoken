using System.Linq.Expressions;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System.Text.RegularExpressions;
namespace Aplicacion.Repository;

    public class UserRepository : GenericRepository<User>, IUser
    {
        private readonly APIContext _context;
        public UserRepository(APIContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<User>> GetAllAsync(){
            return await base.GetAllAsync();
        }
        public override IEnumerable<User> Find(Expression<Func<User, bool>> expression)
        {
            return base.Find(expression);
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Username.ToLower() == username.ToLower());
        }
    public override void Add(User entity)
    {
        Regex regex = new Regex("^(.){1,3}.([aeiou]){1,4}([xyz]){1,5}", RegexOptions.IgnoreCase);
        if(regex.IsMatch(entity.Username)){
            base.Add(entity);
        }
        else{
            throw new Exception("error");
        }
        
    }
    public override void Update(User entities)
    {
        base.Update(entities);
    }
}

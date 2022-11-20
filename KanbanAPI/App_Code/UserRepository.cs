using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.App_Code
{
    public interface IUserRepository
    {
        User GetByID(int id);
        void AddPart(User model);
        Task<User> GetByUserNameAsync(string userName);
        IQueryable<User> GetList();
        Task SaveChangesAsync();
    }
    public class UserRepository : IUserRepository
    {
        internal KanbanDBContext _context;
        internal DbSet<User> _dbSet;
        public UserRepository(KanbanDBContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }
        public User GetByID(int id)
        {
            return _dbSet.SingleOrDefault(x => x.ID == id);
        }
        public void AddPart(User model)
        {
            _context.Entry(model).State = EntityState.Added;
        }
        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserName == userName);
        }
        public IQueryable<User> GetList()
        {
            return _dbSet;
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

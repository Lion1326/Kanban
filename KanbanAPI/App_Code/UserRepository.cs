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

        //Получение юзера по ID
        public User GetByID(int id)
        {
            return _dbSet.SingleOrDefault(x => x.ID == id);
        }

        //Добавление юзера
        public void AddPart(User model)
        {
            _context.Entry(model).State = EntityState.Added;
        }

        //Получение юзера по ID для асинхронного метода
        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        //Получение списка юзеров
        public IQueryable<User> GetList()
        {
            return _dbSet;
        }

        //Сохранение асинхронного метода
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

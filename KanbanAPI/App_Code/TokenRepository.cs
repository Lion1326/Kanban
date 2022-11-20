using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.App_Code
{
    public interface ITokenRepository
    {
        void AddPart(Token model);
        Token Get(int userID, Guid tokenID);
        void DeleteByUserID(int userID);
        void DeletePart(Token model);
        Task SaveChangesAsync();
    }
    public class TokenRepository : ITokenRepository
    {
        internal KanbanDBContext _context;
        internal DbSet<Token> _dbSet;
        public TokenRepository(KanbanDBContext context)
        {
            _context = context;
            _dbSet = context.Set<Token>();
        }

        //Добавление токена
        public void AddPart(Token model)
        {
            _context.Entry(model).State = EntityState.Added;
        }

        //Получени токена по ID и UserID
        public Token Get(int userID, Guid tokenID)
        {
            return _dbSet.FirstOrDefault(x => x.UserID == userID && x.TokenID == tokenID);
        }

        //Удаление токена по UserID после разлогирования
        public void DeleteByUserID(int userID)
        {
            foreach(Token item in _dbSet.Where(x => x.UserID == userID))
            {
                _context.Entry(item).State = EntityState.Deleted;
            }
        }

        //Удаление токена
        public void DeletePart(Token model)
        {
            _context.Entry(model).State = EntityState.Deleted;
        }

        //Сохранение асинхронного метода
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

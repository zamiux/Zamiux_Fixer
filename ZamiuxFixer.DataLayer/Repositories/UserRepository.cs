using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private ZamiuxFixerDbContext _context;
        public UserRepository(ZamiuxFixerDbContext context)
        {
                _context = context;
        }
        #endregion
        public void AddUser(User user)
        {
            _context.Users.Add(user);   
        }

        public void DelteUser(User user)
        {
            _context.Remove(user);
        }

        public List<User> GetAllUser()
        {
            return _context.Users.Where(u=>u.IsDelete == false).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }
    }
}

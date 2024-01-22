using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Repositories
{
    public class UserRepository: GenericRepository<User>,IUserRepository
    {
        #region Constructor
        private readonly ZamiuxFixerDbContext _context;
        public UserRepository(ZamiuxFixerDbContext context) : base(context) 
        {
            _context = context;
        }

        #endregion

        #region isExistUserByEmail
        public bool isExistUserByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        #endregion

        #region Get User By Active Code
        public User GetUserByActiveCode(string code)
        {
            return _context.Users.FirstOrDefault(u => u.ActiveCode == code);
        }


        #endregion

        #region Get User By Email
        public User GetUserByEmail(string Email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == Email);
        }
        #endregion


    }
}

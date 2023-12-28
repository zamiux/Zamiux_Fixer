using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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




    }
}

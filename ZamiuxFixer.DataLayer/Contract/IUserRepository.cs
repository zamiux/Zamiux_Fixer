using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Contract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool isExistUserByEmail(string email); 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Repositories
{
    
    public class RoleRepository : IRoleRepository
    {
        #region Constructor
        private readonly ZamiuxFixerDbContext _context;
        public RoleRepository(ZamiuxFixerDbContext context)
        {
            _context = context;
        }
        #endregion

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
        }

        public void DelteRole(Role role)
        {
            _context.Roles.Remove(role);
        }

        public List<Role> GetAllRoles()
        {
            return _context.Roles.Where(r=>r.IsDelete == false).ToList();
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateRole(Role role)
        {
            _context.Update(role);
        }
    }
}

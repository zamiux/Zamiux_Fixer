using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Contract
{
    public interface IRoleRepository
    {
        // CRUD Role Constracts
        List<Role> GetAllRoles();
        Role GetRoleById(int id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DelteRole(Role role);
        void Save();

    }
}

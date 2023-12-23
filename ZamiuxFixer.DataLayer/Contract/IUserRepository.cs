using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.DataLayer.Contract
{
    public interface IUserRepository
    {
        // CRUD User Constracts
        List<User> GetAllUser();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DelteUser(User user);
        void Save();
    }
}

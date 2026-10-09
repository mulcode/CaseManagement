using CourtDomain.Models;

namespace CourtDataAccess.Interfaces
{
    public interface IUser
    {
        IEnumerable<User> GetAllUser();
        IEnumerable<User> GetUsersByRole(int roleId);
        IEnumerable<User> GetUsersByRoleName(string roleName);
        User GetUser(int userId);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
        User? Login(string userName, string password);

    }
}

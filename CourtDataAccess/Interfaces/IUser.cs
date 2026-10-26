using CourtDomain.Models;

namespace CourtDataAccess.Interfaces
{
    public interface IUser
    {
        IList<User> GetAllUser();
        IList<User> GetUsersByRole(int roleId);
        IList<User> GetUsersByRoleName(string roleName);
        User GetUser(int userId);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
        User? Login(string userName, string password);

    }
}

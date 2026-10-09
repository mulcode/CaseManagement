using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CourtDataAccess.Managers
{
    public class UserManager : IUser
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserManager> _logger;

        public UserManager(
            AppDbContext context,
            ILogger<UserManager> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        public IEnumerable<User> GetAllUser()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users.");
                throw;
            }
        }

        public User GetUser(int id)
        {
            try
            {
                return _context.Users
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting UserId {UserId}",
                    id);

                throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                user.Password = _passwordHasher.HashPassword(
                    user,
                    user.Password
                );

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding user {UserName}",
                    user.UserName);

                throw;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating UserId {UserId}",
                    user.Id);

                throw;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(x => x.Id == id);

                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting UserId {UserId}",
                    id);

                throw;
            }
        }

        public User? Login(string userName, string password)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(x => x.UserName == userName);

                if (user == null)
                {
                    return null;
                }

                var result = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    password
                );

                if (result == PasswordVerificationResult.Success ||
                    result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        user.Password = _passwordHasher.HashPassword(
                            user,
                            password
                        );

                        _context.SaveChanges();
                    }

                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error during login for username {UserName}",
                    userName);

                throw;
            }
        }

        public IEnumerable<User> GetUsersByRole(int roleId)
        {
            try
            {
                return _context.Users
                    .Where(u => u.RoleId == roleId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting users for RoleId {RoleId}",
                    roleId);

                throw;
            }
        }

        public IEnumerable<User> GetUsersByRoleName(string roleName)
        {
            try
            {
                var users =
                    from user in _context.Users
                    join role in _context.Roles
                        on user.RoleId equals role.Id
                    where role.Name == roleName
                    select user;

                return users.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting users for role {RoleName}",
                    roleName);

                throw;
            }
        }
    }
}


using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Identity;


namespace CourtDataAccess.Managers
{
    public class UserManager : IUser
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserManager(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }
       public IEnumerable<User> GetAllUser()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void AddUser(User user)
        {
            user.Password = _passwordHasher.HashPassword(
                  user,
                  user.Password
                   );

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User? Login(string userName, string password)
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

            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }

        public IEnumerable<User> GetUsersByRole(int roleId)
        {
            return _context.Users
                .Where(u => u.RoleId == roleId)
                .ToList();
        }


        //public string GeneratePasswordHash(string password)
        //{
        //    var user = new User
        //    {
        //        UserName = "admin"
        //    };

        //    return _passwordHasher.HashPassword(user, password);
        //}
    }
}

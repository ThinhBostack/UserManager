using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Models;

namespace UserManager.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private readonly UserContext _userContext;

        public SqlUserRepo(UserContext userContext)
        {
            _userContext = userContext;
        }

        public User AddUser(User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
            return user;
        }

        public bool DeleteUser(User user)
        {
            _userContext.Users.Remove(user);
            return _userContext.SaveChanges() > 0;
        }

        public User UpdateUser(User user)
        {
            _userContext.Users.Update(user);
            _userContext.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            return _userContext.Users.ToList();
        }

        public User GetUserByID(string Id)
        {
            return _userContext.Users.FirstOrDefault(p => p.Id.Equals(Id));
        }

        public bool SaveChanges()
        {
            return _userContext.SaveChanges() > 0;
        }
    }
}

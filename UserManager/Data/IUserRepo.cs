using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Models;

namespace UserManager.Data
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUser();

        User GetUserByID(string Id);       

        //User GetUserByID(Guid Id);

        User AddUser(User user);

        bool DeleteUser(User user);

        User UpdateUser(User user);

        bool SaveChanges();
    }
}

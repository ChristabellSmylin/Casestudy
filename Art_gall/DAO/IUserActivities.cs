using Art_gall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art_gall.DAO
{
    internal interface IUserActivities
    {
        List<User> GetAllUsers();
        bool RegisterUser(User user);
        User LoginbyUser(string  username, string password);
    }
}

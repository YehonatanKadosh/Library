using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class User
    {
        public string User_Name { get; private set; }
        public string Password { get; private set; }
        public Level Permission_Level { get; private set; }

        public User(string userName, string password, Level permission_lvl)
        {
            User_Name = userName;
            Password = password;
            Permission_Level = permission_lvl;
        }

        public void UpdateUser(string password, Level permission_level)
        {
            Password = password;
            Permission_Level = permission_level;
        }

        public override string ToString()
        => $"UserName: {User_Name}\nPassword: {Password}\nPremission level: {Permission_Level}";

        public override bool Equals(object obj)
        {
            User other = obj as User;
            return User_Name == other.User_Name;
        }
    }
}


using System;
using CRUD_User.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_User.ViewModel
{
    internal class UserViewModel
    {
        UserModel.User user = new UserModel.User();

        public int getId()
        {
            // Get id from user
            return user.id;
        }

        public void setId(int id)
        {
            // Set id to user
            user.id = id;
        }

        public string getName()
        {
            // Get name from user
            return user.name;
        }

        public void setName(string name)
        {
            // Set name to user
            user.name = name;
        }

        public int getFpId()
        {
            // Get fp_id from user
            return user.fp_id;
        }

        public void setFpId(int fp_id)
        {
            // Set fp_id to user
            user.fp_id = fp_id;
        }
    }
}

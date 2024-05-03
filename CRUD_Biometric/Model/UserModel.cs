using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Biometric.Model
{
    public class UserModel
    {
        public class User
        {
            [Required]
            public int id { get; set; }
            [Required]
            public string name { get; set; }
        }
    }
}

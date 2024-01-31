using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_fingertech.Model
{
    internal class UserModel
    {
        public class User
        {
            [Required]
            public int id { get; set; }
            [Required]
            public string name { get; set; }
            [Required]
            public string fp_id { get; set; }
        }
    }
}

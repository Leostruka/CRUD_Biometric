using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_User.Model
{
    public class FIRModel
    {
        public class FIR
        {
            [Required]
            public int id { get; set; }
            [Required]
            public string hash { get; set; }
            [Required]
            public int sample { get; set; }
        }
    }
}

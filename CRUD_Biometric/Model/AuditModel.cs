using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Biometric.Model
{
    public class AuditModel
    {
        public class Audit
        {
            [Required]
            public int id { get; set; }
            [Required]
            public byte[] data { get; set; }
            [Required]
            public uint imageWidth { get; set; }
            [Required]
            public uint imageHeight { get; set; }
            [Required]
            public int sample { get; set; }
        }
    }
}

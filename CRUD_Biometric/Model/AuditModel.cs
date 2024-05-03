using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Biometric.Model
{
    internal class AuditModel
    {
        public class Audit
        {
            public int id { get; }

            [Required]
            public byte[] data { get; set; }
            [Required]
            public uint imageWidth { get; set; }
            [Required]
            public uint imageHeight { get; set; }
        }
    }
}

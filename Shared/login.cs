using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerProject
{
    public class login
    {
        [Key]
        public int loginNo { get; set; }
        public string? loginUserName { get; set; }
        public string? loginPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerProject
{
    [Serializable]
    public class department
    {
        [Key]
        public int departmentNo { get; set; }

        public string? departmentName { get; set; }

        public string? departmentLocation { get; set; }
    }
}

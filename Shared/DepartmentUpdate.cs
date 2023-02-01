using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerProject
{
    public class DepartmentUpdate
    {
        public int departmentNo { get; set; }

        public string? departmentName { get; set; }

        public string? departmentLocation { get; set; }
        public Boolean? updateClicked { get; set; }
    }
}

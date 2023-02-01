using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerProject
{
    public class EmployeeUpdate
    {
        public int employeeNo { get; set; }
        public string? employeeName { get; set; }
        public int salary { get; set; }
        public int departmentNo { get; set; }
        public DateTime? lastModifyDate { get; set; }
        public Boolean? updateClicked { get; set; }
    }
}

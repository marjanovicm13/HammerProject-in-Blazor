using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammerProject
{
    public class EmployeeStateContainerService
    {
        public List<employee> Value { get; set; }

        public event Action OnStateChange;

        public void SetValue(List<employee> employees)
        {
            Value = employees;
            NotifyStateChanged();
        }

        public void AddValue(employee employee)
        {
            Value.Add(employee);
            NotifyStateChanged();
        }

        public void RemoveValue(int id)
        {
            Value.RemoveAll(x => x.employeeNo == id);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}

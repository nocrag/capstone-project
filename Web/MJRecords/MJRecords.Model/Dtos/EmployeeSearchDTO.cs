using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeSearchDTO
    {
        public int? DepartmentId { get; set; }
        public string? EmployeeId { get; set; }
        public string? LastName { get; set; }
    }
}

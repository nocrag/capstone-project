using Microsoft.AspNetCore.Mvc.Rendering;
using MJRecords.Model;

namespace MJRecords.Web.Models
{
    public class EmployeeUpdateVM
    {
        public EmployeeUpdateDTO Employee { get; set; } = new();
        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        public IEnumerable<SelectListItem> Jobs { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}

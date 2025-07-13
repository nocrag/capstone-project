using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeReviewValidationResultDTO
    {
        public string? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public DateTime SeniorityDate { get; set; }
        public string? MissingReviewPeriod { get; set; }
    }
}

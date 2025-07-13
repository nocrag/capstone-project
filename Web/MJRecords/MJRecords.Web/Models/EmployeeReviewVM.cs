using Microsoft.AspNetCore.Mvc.Rendering;
using MJRecords.Model;

namespace MJRecords.Web.Models
{
    public class EmployeeReviewVM
    {
        public EmployeeReview Review { get; set; } = new();
        public IEnumerable<SelectListItem> Quarter { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> RatingOptions { get; set; }
    }
}

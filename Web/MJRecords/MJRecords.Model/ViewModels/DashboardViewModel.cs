using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class DashboardViewModel
    {
        public List<MonthlyExpenseDto> MonthlyExpenses { get; set; } = new List<MonthlyExpenseDto>();

        public int PendingReviewCount { get; set; }
    }
}

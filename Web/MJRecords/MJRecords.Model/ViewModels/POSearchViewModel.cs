using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model.ViewModels
{
    public class POSearchViewModel
    {
        public POSearchDto Criteria { get; set; } = new();
        public List<POSummaryDto> Results { get; set; } = new();
    }
}

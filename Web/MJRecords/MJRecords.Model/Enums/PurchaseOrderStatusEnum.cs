using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public enum PurchaseOrderStatusEnum
    {
        Pending = 0,

        [Display(Name = "Under Review")]
        UnderReview = 1,

        Closed = 2
    }
}

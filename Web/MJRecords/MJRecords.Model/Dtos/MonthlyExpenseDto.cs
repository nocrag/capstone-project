﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class MonthlyExpenseDto
    {
        public string Month { get; set; }
        public decimal Total { get; set; }

        public int? POCount { get; set; }
    }
}

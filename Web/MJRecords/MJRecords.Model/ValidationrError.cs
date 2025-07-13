using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJRecords.Types;

namespace MJRecords.Model
{
    public class ValidationrError
    {
        public ValidationrError(string desc, ErrorType errType)
        {
            Description = desc;
            ErrorType = errType;
        }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}

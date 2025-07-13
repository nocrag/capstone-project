using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public abstract class BaseEntity
    {
        public List<ValidationrError> Errors { get; set; } = new();

        public void AddError(ValidationrError error)
        {
            Errors.Add(error);
        }
    }
}

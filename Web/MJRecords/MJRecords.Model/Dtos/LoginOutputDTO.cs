using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class LoginOutputDTO
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public char? MiddleInitial { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }

        public int DepartmentId { get; set; }
    }
}

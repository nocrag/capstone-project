using DAL;
using MJRecords.Model;
using MJRecords.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public class LoginRepo : ILoginRepo
    {
        private readonly IDataAccess _db;

        public LoginRepo(IDataAccess db)
        {
            _db = db;
        }

        public byte[] GetEmployeeSalt(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id)
            };

            DataTable dt = _db.Execute("spGetEmployeeSalt", parms);

            if (dt.Rows.Count > 0)
            {
                return (byte[])dt.Rows[0]["PasswordSalt"];
            }

            return null;
        }

        public async Task<byte[]> GetEmployeeSaltAsync(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmployeeSalt", parms);

            if (dt.Rows.Count > 0)
            {
                return (byte[])dt.Rows[0]["PasswordSalt"];
            }

            return null;
        }

        public Employee Login(LoginDTO loginDTO)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, loginDTO.Username),
                new("@Password", SqlDbType.Char, loginDTO.Password),
            };

            DataTable dt = _db.Execute("spVerifyLogin", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return PopulateEmployeeFromDataRow(row);
            }

            return null;
        }

        public async Task<Employee> LoginAsync(LoginDTO loginDTO)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, loginDTO.Username),
                new("@Password", SqlDbType.Char, loginDTO.Password),
            };

            DataTable dt = await _db.ExecuteAsync("spVerifyLogin", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return PopulateEmployeeFromDataRow(row);
            }

            return null;
        }

        

        private Employee PopulateEmployeeFromDataRow(DataRow row)
        {
            return new Employee
            {
                Id = row["Id"].ToString(),
                SupervisorId = row["SupervisorId"].ToString(),
                DepartmentId = int.TryParse(row["DepartmentId"]?.ToString(), out var deptId) ? deptId : (int?)null,
                JobAssignmentId = Convert.ToInt32(row["JobAssignmentId"]),
                Status = Convert.ToInt32(row["EmploymentStatusId"]),
                Password = row["Password"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                MiddleInitial = row["MiddleInitial"] == DBNull.Value ? (char?)null : Convert.ToChar(row["MiddleInitial"]),
                StreetAddress = row["StreetAddress"].ToString(),
                City = row["City"].ToString(),
                Province = row["Province"].ToString(),
                PostalCode = row["PostalCode"].ToString(),
                DateOfBirth = Convert.ToDateTime(row["DOB"]),
                SIN = row["SIN"].ToString(),
                SeniorityDate = Convert.ToDateTime(row["SeniorityDate"]),
                JobStartDate = Convert.ToDateTime(row["JobStartDate"]),
                WorkPhone = row["WorkPhone"].ToString(),
                CellPhone = row["CellPhone"].ToString(),
                EmailAddress = row["EmailAddress"].ToString(),
                OfficeLocation = row["OfficeLocation"].ToString()
            };
        }
    }
}

using DAL;
using MJRecords.Model;
using MJRecords.Types;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public class EmploymentStatusRepo : IEmploymentStatusRepo
    {
        private readonly IDataAccess _db;

        public EmploymentStatusRepo(IDataAccess db)
        {
            _db = db;
        }

        public EmploymentStatus Get(int statusId)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, statusId)
            };

            DataTable dt = _db.Execute("spGetEmploymentStatus", parms);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new EmploymentStatus
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Status = row["Status"].ToString(),
                };
            }

            return null;
        }

        public List<EmploymentStatus> GetAll()
        {
            DataTable dt = _db.Execute("spGetAllEmploymentStatus");
            return dt.AsEnumerable().Select(row => new EmploymentStatus
            {
                Id = Convert.ToInt32(row["Id"]),
                Status = row["Status"].ToString()
            }).ToList();
        }

        public async Task<List<EmploymentStatus>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllEmploymentStatus");
            return dt.AsEnumerable().Select(row => new EmploymentStatus
            {
                Id = Convert.ToInt32(row["Id"]),
                Status = row["Status"].ToString()
            }).ToList();
        }

        public async Task<EmploymentStatus> GetAsync(int statusId)
        {

            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, statusId)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmploymentStatus", parms);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new EmploymentStatus
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Status = row["Status"].ToString(),
                };
            }

            return null;
        }
    }
}

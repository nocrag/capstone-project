using DAL;
using MJRecords.Model;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public class JobRepo : IJobRepo
    {
        private readonly IDataAccess _db;

        public JobRepo(IDataAccess db)
        {
            _db = db;
        }

        public Job Get(int id)
        {
            List<Parm> parms = new()
            {
                new("Id", SqlDbType.Int, id)
            };

            DataTable dt = _db.Execute("spGetJob", parms);

            if (dt.Rows.Count > 0)
            {
                return new Job { Id = Convert.ToInt32(dt.Rows[0]["Id"]), Title = dt.Rows[0]["Title"].ToString() };
            }

            return null;
        }

        public async Task<Job> GetAsync(int id)
        {
            List<Parm> parms = new()
            {
                new("Id", SqlDbType.Int, id)
            };

            DataTable dt = await _db.ExecuteAsync("spGetJob", parms);

            if (dt.Rows.Count > 0)
            {
                return new Job { Id = Convert.ToInt32(dt.Rows[0]["Id"]), Title = dt.Rows[0]["Title"].ToString() };
            }

            return null;
        }

        public List<Job> GetAll()
        {
            DataTable dt = _db.Execute("spGetAllJobs");

            return dt.AsEnumerable().Select(row => new Job { Id=Convert.ToInt32(row["Id"]), Title=row["Title"].ToString() }).ToList();
        }

        public async Task<List<Job>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllJobs");

            return dt.AsEnumerable().Select(row => new Job { Id = Convert.ToInt32(row["Id"]), Title = row["Title"].ToString() }).ToList();
        }
  
    }
}

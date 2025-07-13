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
    public class RatingOptionsRepo : IRatingOptionsRepo
    {
        private readonly IDataAccess _db;

        public RatingOptionsRepo(IDataAccess db)
        {
            _db = db;
        }

        public RatingOptions Get(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id)
            };

            DataTable dt = _db.Execute("spGetRatingOptionsById", parms);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return new RatingOptions
                {
                    Id = Convert.ToInt32(row["Id"]),
                    RatingOption = row["Rating"].ToString(),
                };
            }

            return null;
        }

        public List<RatingOptions> GetAll()
        {
            DataTable dt = _db.Execute("spGetRatingOptions");

            return dt.AsEnumerable().Select(row => new RatingOptions
            {
                Id = Convert.ToInt32(row["Id"]),
                RatingOption = row["Rating"].ToString()
            }).ToList();
        }

        public async Task<List<RatingOptions>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetRatingOptions");

            return dt.AsEnumerable().Select(row => new RatingOptions
            {
                Id = Convert.ToInt32(row["Id"]),
                RatingOption = row["Rating"].ToString()
            }).ToList();
        }

        public async Task<RatingOptions> GetAsync(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id)
            };

            DataTable dt = await _db.ExecuteAsync("spGetRatingOptionsById", parms);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return new RatingOptions
                {
                    Id = Convert.ToInt32(row["Id"]),
                    RatingOption = row["Rating"].ToString(),
                };
            }

            return null;
        }
    }
}

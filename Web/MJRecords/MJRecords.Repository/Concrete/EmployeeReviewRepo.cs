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
    public class EmployeeReviewRepo : IEmployeeReviewRepo
    {
        private readonly IDataAccess _db;

        public EmployeeReviewRepo(IDataAccess db)
        {
            _db = db;
        }

        public EmployeeReviewDTO Get(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id)
            };

            DataTable dt = _db.Execute("spGetEmpReviewById", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return PopulateEmpReviewDtoFromDataRow(row);
            }

            return null;
        }
        public async Task<EmployeeReviewDTO> GetAsync(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmpReviewById", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return PopulateEmpReviewDtoFromDataRow(row);
            }

            return null;
        }

        public List<EmployeeReviewDTO> GetAll()
        {
            DataTable dt = _db.Execute("spGetAllEmpReviews");

            return dt.AsEnumerable().Select(row => PopulateEmpReviewDtoFromDataRow(row)).ToList();
        }

        public async Task<List<EmployeeReviewDTO>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllEmpReviews");

            return dt.AsEnumerable().Select(row => PopulateEmpReviewDtoFromDataRow(row)).ToList();
        }

        public EmployeeReview Create(EmployeeReview empReview, int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@EmployeeId", SqlDbType.Char, empReview.EmployeeId),
                new("@RatingOptionsId", SqlDbType.Int, empReview.RatingOptionsId),
                new("@Comment", SqlDbType.VarChar, empReview.Comment),
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year),
                new("@ReviewDate", SqlDbType.DateTime, empReview.ReviewDate)
            };

            if (_db.ExecuteNonQuery("spAddEmployeeReviewForQuarter", parms) <= 0)
            {
                throw new DataException("There was an issue with adding the employee review to the database.");
            }

            return empReview;
        }

        public async Task<EmployeeReview> CreateAsync(EmployeeReview empReview, int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@EmployeeId", SqlDbType.Char, empReview.EmployeeId),
                new("@RatingOptionsId", SqlDbType.Int, empReview.RatingOptionsId),
                new("@Comment", SqlDbType.VarChar, empReview.Comment),
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year),
                new("@ReviewDate", SqlDbType.DateTime, empReview.ReviewDate)
            };

            if (await _db.ExecuteNonQueryAsync("spAddEmployeeReviewForQuarter", parms) <= 0)
            {
                throw new DataException("There was an issue with adding the employee review to the database.");
            }

            return empReview;
        }

        private EmployeeReviewDTO PopulateEmpReviewDtoFromDataRow(DataRow row)
        {
            return new EmployeeReviewDTO
            {
                Id = Convert.ToInt32(row["Id"]),
                EmployeeId = row["EmployeeId"].ToString(),
                RatingOptionsId = Convert.ToInt32(row["RatingOptionsId"]),
                Comment = row["Comment"].ToString(),
                ReviewDate = Convert.ToDateTime(row["ReviewDate"])
            };
        }

        public List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarter(int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year)
            };

            DataTable dt = _db.Execute("spFindEmployeesWithoutReviewInQuarter", parms);

            if (dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new EmployeeReviewValidationResultDTO
                {
                    EmployeeId = row["Id"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    JobTitle = row["JobTitle"].ToString(),
                    SeniorityDate = Convert.ToDateTime(row["SeniorityDate"]),
                    MissingReviewPeriod = row["MissingReviewPeriod"].ToString()
                }).ToList();
            }

            return null;
        }

        public async Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterAsync(int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year)
            };

            DataTable dt = await _db.ExecuteAsync("spFindEmployeesWithoutReviewInQuarter", parms);

            if (dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new EmployeeReviewValidationResultDTO
                {
                    EmployeeId = row["Id"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    JobTitle = row["JobTitle"].ToString(),
                    SeniorityDate = Convert.ToDateTime(row["SeniorityDate"]),
                    MissingReviewPeriod = row["MissingReviewPeriod"].ToString()
                }).ToList();
            }

            return null;
        }

        public List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarterBySupervisor(string id, int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id),
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year)
            };

            DataTable dt = _db.Execute("spFindEmployeesWithoutReviewInQuarterBySupervisor", parms);

            if (dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new EmployeeReviewValidationResultDTO
                {
                    EmployeeId = row["Id"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    JobTitle = row["JobTitle"].ToString(),
                    SeniorityDate = Convert.ToDateTime(row["SeniorityDate"]),
                    MissingReviewPeriod = row["MissingReviewPeriod"].ToString()
                }).ToList();
            }

            return null;
        }

        public async Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterBySupervisorAsync(string id, int quarter, int year)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id),
                new("@Quarter", SqlDbType.Int, quarter),
                new("@Year", SqlDbType.Int, year)
            };

            DataTable dt = await _db.ExecuteAsync("spFindEmployeesWithoutReviewInQuarterBySupervisor", parms);

            if (dt.Rows.Count > 0)
            {
                return dt.AsEnumerable().Select(row => new EmployeeReviewValidationResultDTO
                {
                    EmployeeId = row["Id"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    JobTitle = row["JobTitle"].ToString(),
                    SeniorityDate = Convert.ToDateTime(row["SeniorityDate"]),
                    MissingReviewPeriod = row["MissingReviewPeriod"].ToString()
                }).ToList();
            }

            return null;
        }

        public List<EmployeeReviewListDTO> GetAllReviewsMadeBySupervisor(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id)
            };

            DataTable dt = _db.Execute("spFindAllReviewsMadeBySupervisor", parms);

            return dt.AsEnumerable().Select(row => PopulateEmployeeReviewListDTOFromDataRow(row)).ToList();
        }

        public async Task<List<EmployeeReviewListDTO>> GetAllReviewsMadeBySupervisorAsync(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id)
            };

            DataTable dt = await _db.ExecuteAsync("spFindAllReviewsMadeBySupervisor", parms);

            return dt.AsEnumerable().Select(row => PopulateEmployeeReviewListDTOFromDataRow(row)).ToList();
        }

        private EmployeeReviewListDTO PopulateEmployeeReviewListDTOFromDataRow(DataRow row)
        {
            return new EmployeeReviewListDTO
            {
                Id = Convert.ToInt32(row["Id"]),
                EmployeeFName = row["FirstName"].ToString(),
                EmployeeLName = row["LastName"].ToString(),
                SupervisorFName = row["SupervisorFName"].ToString(),
                SupervisorLName = row["SupervisorLName"].ToString(),
                Rating = row["Rating"].ToString(),
                Comment = row["Comment"].ToString(),
                ReviewDate = Convert.ToDateTime(row["ReviewDate"]),
                YearQuarter = row["Quarter"].ToString(),
            };
        }
    }
}

using DAL;
using Microsoft.Data.SqlClient;
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
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly IDataAccess _db;

        public DepartmentRepo(IDataAccess db)
        {
            _db = db;
        }

        public List<DepartmentDTO> GetAll()
        {
            DataTable dt = _db.Execute("spGetAllDepartments");

            return dt.AsEnumerable().Select(row => PopulateDepartmentDTOFromRow(row)).ToList();
        }

        public async Task<List<DepartmentDTO>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllDepartments");

            return dt.AsEnumerable().Select(row => PopulateDepartmentDTOFromRow(row)).ToList();
        }

        public DepartmentDTO Get(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id),
            };

            DataTable dt = _db.Execute("spGetDepartment", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateDepartmentDTOFromRow(dt.Rows[0]);
            }
            return null;
        }

        public Department GetDept(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id),
            };

            DataTable dt = _db.Execute("spGetDepartment", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                return new Department
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Description = dr["Description"].ToString(),
                    Name = dr["Name"].ToString(),
                    InvocationDate = Convert.ToDateTime(dr["InvocationDate"]),
                    RecordVersion = (byte[])dr["RecordVersion"]
                };
            }
            return null;
        }

        public async Task<DepartmentDTO> GetAsync(int id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, id),
            };

            DataTable dt = await _db.ExecuteAsync("spGetDepartment", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateDepartmentDTOFromRow(dt.Rows[0]);
            }
            return null;
        }

        public Department Create(Department department)
        {
            try
            {
                List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, department.Id, 0, ParameterDirection.Output),
                new("@Name", SqlDbType.VarChar, department.Name),
                new("@Description", SqlDbType.VarChar, department.Description),
                new("@InvocationDate", SqlDbType.DateTime, department.InvocationDate),
            };

                if (_db.ExecuteNonQuery("spCreateDepartment", parms) > 0)
                {
                    department.Id = (int?)parms.FirstOrDefault(p => p.Name == "@Id")!.Value ?? 0;
                    return department;
                }

                department.AddError(new("There was an issue adding the record to the database.", ErrorType.Business));

                //return department;
            }
            catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }
        public async Task<Department> CreateAsync(Department department)
        {
            try
            {
                List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Int, department.Id, 0, ParameterDirection.Output),
                new("@Name", SqlDbType.VarChar, department.Name),
                new("@Description", SqlDbType.VarChar, department.Description),
                new("@InvocationDate", SqlDbType.DateTime, department.InvocationDate),
            };

                if (await _db.ExecuteNonQueryAsync("spCreateDepartment", parms) > 0)
                {
                    department.Id = (int?)parms.FirstOrDefault(p => p.Name == "@Id")!.Value ?? 0;
                    return department;
                }

                department.AddError(new("There was an issue adding the record to the database.", ErrorType.Business));

                //return department;
            }
            catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }

        public Department Update(Department department)
        {
            try
            {
                List<Parm> parms = new()
                {
                    new("@Id" , SqlDbType.Int, department.Id),
                    new("@Name" , SqlDbType.VarChar, department.Name),
                    new("@Description" , SqlDbType.VarChar, department.Description),
                    new("@InvocationDate" , SqlDbType.DateTime, department.InvocationDate),
                    new("@RecordVersion" , SqlDbType.Timestamp, department.RecordVersion)
                };

                if(_db.ExecuteNonQuery("spUpdateDepartment", parms) > 0)
                {
                    return department;
                }

                department.AddError(new("There was an issue while updating the record in the database.", ErrorType.Business));

                //return department;
            }
            catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            try
            {
                List<Parm> parms = new()
                {
                    new("@Id" , SqlDbType.Int, department.Id),
                    new("@Name" , SqlDbType.VarChar, department.Name),
                    new("@Description" , SqlDbType.VarChar, department.Description),
                    new("@InvocationDate" , SqlDbType.DateTime, department.InvocationDate),
                    new("@RecordVersion" , SqlDbType.Timestamp, department.RecordVersion)
                };

                if (await _db.ExecuteNonQueryAsync("spUpdateDepartment", parms) > 0)
                {
                    return department;
                }

                department.AddError(new("There was an issue while updating the record in the database.", ErrorType.Business));

                //return department;
            }
            catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }

        public Department Delete(Department department)
        {
            try
            {
                List<Parm> parms = new()
                {
                new("@Id" , SqlDbType.Int, department.Id),
                new("@RecordVersion", SqlDbType.Timestamp, department.RecordVersion)
                };

                if (_db.ExecuteNonQuery("spDeleteDepartment", parms) > 0)
                {
                    return department;
                }

                department.AddError(new("There was an issue while deleting the record from the database.", ErrorType.Business));
            } catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }

        public async Task<Department> DeleteAsync(Department department)
        {
            try
            {
                List<Parm> parms = new()
                {
                new("@Id" , SqlDbType.Int, department.Id),
                new("@RecordVersion", SqlDbType.Timestamp, department.RecordVersion)
                };

                if (await _db.ExecuteNonQueryAsync("spDeleteDepartment", parms) > 0)
                {
                    return department;
                }

                department.AddError(new("There was an issue while deleting the record from the database.", ErrorType.Business));
            }
            catch (SqlException ex)
            {
                department.AddError(new(ex.Message, ErrorType.Business));
            }

            return department;
        }

        private DepartmentDTO PopulateDepartmentDTOFromRow(DataRow row)
        {
            return new DepartmentDTO
            {
                Id = Convert.ToInt32(row["ID"]),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                InvocationDate = Convert.ToDateTime(row["InvocationDate"])
            };
        }

        public Department PopulateDepartmentFromDTO(DepartmentDTO deptDTO)
        {
            return new Department
            {
                Id = deptDTO.Id,
                Name = deptDTO.Name,
                Description = deptDTO.Description,
                InvocationDate = deptDTO.InvocationDate,
            };
        }

        public bool IsDepartmentNameUnique(string name)
        {
            List<Parm> parms = new()
            {
                new("@Name", SqlDbType.VarChar, name)
            };

            DataTable dt = _db.Execute("spGetDepartmentByName", parms);

            if (dt.Rows.Count == 0)
            {
                // The name wasnt found in the DB, that means the name is unique
                return true;
            }

            return false;
        }
    }
}

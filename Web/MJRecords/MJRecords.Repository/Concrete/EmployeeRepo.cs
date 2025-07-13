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
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly IDataAccess _db;

        public EmployeeRepo(IDataAccess db)
        {
            _db = db;
        }

        public async Task<List<EmployeeDTO>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllEmployees");

            return dt.AsEnumerable().Select(row => PopulateEmployeeDTOFromDataRow(row)).ToList();

        }

        public List<EmployeeDTO> GetAll()
        {
            DataTable dt = _db.Execute("spGetAllEmployees");

            return dt.AsEnumerable().Select(row => PopulateEmployeeDTOFromDataRow(row)).ToList();

        }

        public async Task<EmployeeDTO> GetAsync(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmployee", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateEmployeeDTOFromDataRow(dt.Rows[0]);
            }

            return null;
        }

        public Employee GetEmp(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = _db.Execute("spGetEmployee", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateEmployeeFromDataRow(dt.Rows[0]);
            }

            return null;
        }

        public async Task<Employee> GetEmpAsync(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmployee", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateEmployeeFromDataRow(dt.Rows[0]);
            }

            return null;
        }

        public EmployeeDTO Get(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = _db.Execute("spGetEmployee", parms);

            if (dt.Rows.Count > 0)
            {
                return PopulateEmployeeDTOFromDataRow(dt.Rows[0]);
            }

            return null;
        }

        public async Task<Employee> CreateAsync(EmployeeDTO empDto)
        {
            Employee? lastEmployeeCreated = await GetLastEmployeeAsync();

            int newId;
            if (lastEmployeeCreated != null)
            {
                newId = Convert.ToInt32(lastEmployeeCreated.Id) + 1;
            }
            else
            {
                newId = 1; // start at id 1 if there are no previous records
            }

            empDto.Id = newId.ToString().PadLeft(8, '0').Substring(0, 8);

            Employee emp = PopulateEmployee(empDto);

            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, emp.Id, 8, ParameterDirection.Output),
                new("@EmploymentStatusId", SqlDbType.Int, emp.Status),
                new("@SupervisorId", SqlDbType.Char, emp.SupervisorId),
                new("@DepartmentId", SqlDbType.Int, emp.DepartmentId),
                new("@JobAssignmentId", SqlDbType.Int, emp.JobAssignmentId),
                new("@Password",     SqlDbType.VarChar, emp.Password),
                new("@PasswordSalt", SqlDbType.Binary, emp.PasswordSalt),
                new("@FirstName", SqlDbType.VarChar, emp.FirstName),
                new("@LastName", SqlDbType.VarChar, emp.LastName),
                new("@MiddleInitial", SqlDbType.Char, emp.MiddleInitial),
                new("@StreetAddress", SqlDbType.VarChar, emp.StreetAddress),
                new("@City", SqlDbType.VarChar, emp.City),
                new("@Province", SqlDbType.VarChar, emp.Province),
                new("@PostalCode", SqlDbType.VarChar, emp.PostalCode),
                new("@DOB", SqlDbType.DateTime, emp.DateOfBirth),
                new("@SIN", SqlDbType.VarChar, emp.SIN),
                new("@SeniorityDate", SqlDbType.DateTime, emp.SeniorityDate),
                new("@JobStartDate", SqlDbType.DateTime, emp.JobStartDate),
                new("@WorkPhone", SqlDbType.VarChar, emp.WorkPhone),
                new("@CellPhone", SqlDbType.VarChar, emp.CellPhone),
                new("@EmailAddress", SqlDbType.VarChar, emp.EmailAddress),
                new("@OfficeLocation", SqlDbType.VarChar, emp.OfficeLocation),
                new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion, 0, ParameterDirection.Output)
            };

            if (await _db.ExecuteNonQueryAsync("spAddEmployee", parms) > 0)
            {
                emp.Id = (string?)parms.FirstOrDefault(p => p.Name == "@Id")!.Value ?? string.Empty;
                emp.RecordVersion = (byte[]?)parms.FirstOrDefault(p => p.Name == "@RecordVersion")!.Value;
            }

            else
                throw new DataException("There was an issue with adding the employee to the database.");

            return emp;
        }

        public Employee Create(EmployeeDTO empDto)
        {
            Employee? lastEmployeeCreated = GetLastEmployee();

            int newId;
            if (lastEmployeeCreated != null)
            {
                newId = Convert.ToInt32(lastEmployeeCreated.Id) + 1;
            }
            else
            {
                newId = 1; // start at id 1 if there are no previous records
            }

            empDto.Id = newId.ToString().PadLeft(8, '0').Substring(0, 8);

            Employee emp = PopulateEmployee(empDto);

            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, emp.Id, 8, ParameterDirection.InputOutput),
                new("@EmploymentStatusId", SqlDbType.Int, emp.Status),
                new("@SupervisorId", SqlDbType.Char, emp.SupervisorId),
                new("@DepartmentId", SqlDbType.Int, emp.DepartmentId),
                new("@JobAssignmentId", SqlDbType.Int, emp.JobAssignmentId),
                new("@Password",     SqlDbType.VarChar, emp.Password),
                new("@PasswordSalt", SqlDbType.Binary, emp.PasswordSalt),
                new("@FirstName", SqlDbType.VarChar, emp.FirstName),
                new("@LastName", SqlDbType.VarChar, emp.LastName),
                new("@MiddleInitial", SqlDbType.Char, emp.MiddleInitial),
                new("@StreetAddress", SqlDbType.VarChar, emp.StreetAddress),
                new("@City", SqlDbType.VarChar, emp.City),
                new("@Province", SqlDbType.VarChar, emp.Province),
                new("@PostalCode", SqlDbType.VarChar, emp.PostalCode),
                new("@DOB", SqlDbType.DateTime, emp.DateOfBirth),
                new("@SIN", SqlDbType.VarChar, emp.SIN),
                new("@SeniorityDate", SqlDbType.DateTime, emp.SeniorityDate),
                new("@JobStartDate", SqlDbType.DateTime, emp.JobStartDate),
                new("@WorkPhone", SqlDbType.VarChar, emp.WorkPhone),
                new("@CellPhone", SqlDbType.VarChar, emp.CellPhone),
                new("@EmailAddress", SqlDbType.VarChar, emp.EmailAddress),
                new("@OfficeLocation", SqlDbType.VarChar, emp.OfficeLocation),
                new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion, 0, ParameterDirection.Output)
            };

            if (_db.ExecuteNonQuery("spAddEmployee", parms) > 0)
            {
                // emp.Id = (string?)parms.FirstOrDefault(p => p.Name == "@Id")!.Value ?? string.Empty;
                emp.RecordVersion = (byte[]?)parms.FirstOrDefault(p => p.Name == "@RecordVersion")!.Value;
            }

            else
                throw new DataException("There was an issue with adding the employee to the database.");

            return emp;
        }

        public async Task<Employee> DeleteAsync(Employee emp)
        {
            try
            {
                List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, emp.Id),
                new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion),
            };

                if (await _db.ExecuteNonQueryAsync("spDeleteEmployee", parms) > 0)
                {
                    return emp;
                }

                emp.AddError(new("Unknown error occurred during deletion.", ErrorType.Business));
            }
            catch (SqlException ex)
            {
                // Catch sql exception
                emp.AddError(new(ex.Message, ErrorType.Business));
            }

            return emp;
        }

        public Employee Delete(Employee emp)
        {
            try
            {
                List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, emp.Id),
                new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion),
            };

                if (_db.ExecuteNonQuery("spDeleteEmployee", parms) > 0)
                {
                    return emp;
                }

                emp.AddError(new("Unknown error occurred during deletion.", ErrorType.Business));
            }
            catch (SqlException ex)
            {
                // Catch sql exception
                emp.AddError(new(ex.Message, ErrorType.Business));
            }

            return emp;
        }

        public async Task<Employee> UpdateAsync(Employee emp)
        {
            try
            {
                List<Parm> parms = new()
                {
                    new("@Id", SqlDbType.Char, emp.Id),
                    new("@EmploymentStatusId", SqlDbType.Int, emp.Status),
                    new("@SupervisorId", SqlDbType.Char, emp.SupervisorId),
                    new("@DepartmentId", SqlDbType.Int, emp.DepartmentId),
                    new("@JobAssignmentId", SqlDbType.Int, emp.JobAssignmentId),
                    new("@Password", SqlDbType.VarChar, emp.Password),
                    new("@PasswordSalt", SqlDbType.Binary, emp.PasswordSalt),
                    new("@FirstName", SqlDbType.VarChar, emp.FirstName),
                    new("@LastName", SqlDbType.VarChar, emp.LastName),
                    new("@MiddleInitial", SqlDbType.Char, emp.MiddleInitial),
                    new("@StreetAddress", SqlDbType.VarChar, emp.StreetAddress),
                    new("@City", SqlDbType.VarChar, emp.City),
                    new("@Province", SqlDbType.VarChar, emp.Province),
                    new("@PostalCode", SqlDbType.VarChar, emp.PostalCode),
                    new("@DOB", SqlDbType.DateTime, emp.DateOfBirth),
                    new("@SIN", SqlDbType.VarChar, emp.SIN),
                    new("@SeniorityDate", SqlDbType.Date, emp.SeniorityDate),
                    new("@JobStartDate", SqlDbType.DateTime, emp.JobStartDate),
                    new("@WorkPhone", SqlDbType.VarChar, emp.WorkPhone),
                    new("@CellPhone", SqlDbType.VarChar, emp.CellPhone),
                    new("@EmailAddress", SqlDbType.VarChar, emp.EmailAddress),
                    new("@OfficeLocation", SqlDbType.VarChar, emp.OfficeLocation),
                    new("@RetirementDate", SqlDbType.DateTime, emp.RetirementDate),
                    new("@TerminationDate", SqlDbType.DateTime, emp.TerminationDate),
                    new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion)
                };

                if(await _db.ExecuteNonQueryAsync("spUpdateEmployee", parms) > 0)
                {
                    return emp;
                }

                emp.AddError(new("Unknown error occurred during update.", ErrorType.Business));
            }
            catch (SqlException ex)
            {
                // Catch sql exception
                emp.AddError(new(ex.Message, ErrorType.Business));
            }

            return emp;
        }

        public Employee Update(Employee emp)
        {
            try
            {
                List<Parm> parms = new()
                {
                    new("@Id", SqlDbType.Char, emp.Id),
                    new("@EmploymentStatusId", SqlDbType.Int, emp.Status),
                    new("@SupervisorId", SqlDbType.Char, emp.SupervisorId),
                    new("@DepartmentId", SqlDbType.Int, emp.DepartmentId),
                    new("@JobAssignmentId", SqlDbType.Int, emp.JobAssignmentId),
                    new("@Password", SqlDbType.VarChar, emp.Password),
                    new("@PasswordSalt", SqlDbType.Binary, emp.PasswordSalt),
                    new("@FirstName", SqlDbType.VarChar, emp.FirstName),
                    new("@LastName", SqlDbType.VarChar, emp.LastName),
                    new("@MiddleInitial", SqlDbType.Char, emp.MiddleInitial),
                    new("@StreetAddress", SqlDbType.VarChar, emp.StreetAddress),
                    new("@City", SqlDbType.VarChar, emp.City),
                    new("@Province", SqlDbType.VarChar, emp.Province),
                    new("@PostalCode", SqlDbType.VarChar, emp.PostalCode),
                    new("@DOB", SqlDbType.DateTime, emp.DateOfBirth),
                    new("@SIN", SqlDbType.VarChar, emp.SIN),
                    new("@SeniorityDate", SqlDbType.Date, emp.SeniorityDate),
                    new("@JobStartDate", SqlDbType.DateTime, emp.JobStartDate),
                    new("@WorkPhone", SqlDbType.VarChar, emp.WorkPhone),
                    new("@CellPhone", SqlDbType.VarChar, emp.CellPhone),
                    new("@EmailAddress", SqlDbType.VarChar, emp.EmailAddress),
                    new("@OfficeLocation", SqlDbType.VarChar, emp.OfficeLocation),
                    new("@RetirementDate", SqlDbType.DateTime, emp.RetirementDate),
                    new("@TerminationDate", SqlDbType.DateTime, emp.TerminationDate),
                    new("@RecordVersion", SqlDbType.Timestamp, emp.RecordVersion)
                };

                if (_db.ExecuteNonQuery("spUpdateEmployee", parms) > 0)
                {
                    return emp;
                }

                emp.AddError(new("Unknown error occurred during update.", ErrorType.Business));
            }
            catch (SqlException ex)
            {
                // Catch sql exception
                emp.AddError(new(ex.Message, ErrorType.Business));
            }

            return emp;
        }

        public EmployeeDetailsDto? GetEmployeeDetails(string employeeId)
        {
            var parms = new List<Parm>
            {
                new("@EmployeeId", SqlDbType.Char, employeeId)
            };

            var dt = _db.Execute("spGetEmployeeDetails", parms);
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];

            return new EmployeeDetailsDto
            {
                FullName = row["FullName"].ToString()!,
                Department = row["Department"].ToString()!,
                SupervisorFullName = row["SupervisorFullName"].ToString()!
            };
        }

        public async Task<EmployeeDetailsDto?> GetEmployeeDetailsAsync(string employeeId)
        {
            var parms = new List<Parm>
            {
                new("@EmployeeId", SqlDbType.Char, employeeId)
            };

            var dt = await _db.ExecuteAsync("spGetEmployeeDetails", parms);
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];

            return new EmployeeDetailsDto
            {
                FullName = row["FullName"].ToString()!,
                Department = row["Department"].ToString()!,
                SupervisorFullName = row["SupervisorFullName"].ToString()!
            };
        }

        /// <summary>
        /// Asynchronously retrieves the most recently inserted employee from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the most recently inserted employee if found; otherwise, null.</returns>
        private async Task<Employee?> GetLastEmployeeAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetLastEmployee");

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];

            return PopulateEmployeeFromDataRow(row);

        }

        public List<EmployeeSearchResultDTO?> SearchEmployee(EmployeeSearchDTO searchDto)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, searchDto.DepartmentId),
                new("@EmployeeID", SqlDbType.Char, searchDto.EmployeeId),
                new("@LastName", SqlDbType.VarChar, searchDto.LastName)
            };

            DataTable dt = _db.Execute("spSearchEmployee", parms);

            return dt.AsEnumerable().Select(row => new EmployeeSearchResultDTO
            {
                Id = row["Id"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                Position = row["JobTitle"].ToString(),
                OfficeLocation = row["OfficeLocation"].ToString(),
                WorkPhone = row["WorkPhone"].ToString()
            }).ToList()!;

        }

        public async Task<List<EmployeeSearchResultDTO?>> SearchEmployeeAsync(EmployeeSearchDTO searchDto)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, searchDto.DepartmentId),
                new("@EmployeeID", SqlDbType.Char, searchDto.EmployeeId),
                new("@LastName", SqlDbType.VarChar, searchDto.LastName)
            };

            DataTable dt = await _db.ExecuteAsync("spSearchEmployee", parms);

            return dt.AsEnumerable().Select(row => new EmployeeSearchResultDTO
            {
                Id = row["Id"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                Position = row["JobTitle"].ToString(),
                OfficeLocation = row["OfficeLocation"].ToString(),
                WorkPhone = row["WorkPhone"].ToString()
            }).ToList()!;
        }

        /// <summary>
        /// Retrieves the most recently inserted employee from the database.
        /// </summary>
        /// <returns>Returns the most recently inserted employee if found; otherwise, null.</returns>
        private Employee? GetLastEmployee()
        {
            DataTable dt = _db.Execute("spGetLastEmployee");

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];

            return PopulateEmployeeFromDataRow(row);

        }

        /// <summary>
        /// Creates an Employee entity by mapping values from a DataRow.
        /// </summary>
        /// <param name="row">The DataRow containing employee data.</param>
        /// <returns>A populated Employee entity with values from the DataRow.</returns>
        private Employee PopulateEmployeeFromDataRow(DataRow row)
        {
            return new Employee
            {
                Id = row["Id"]?.ToString(),
                SupervisorId = row["SupervisorId"] != DBNull.Value ? row["SupervisorId"].ToString() : null,
                DepartmentId = int.TryParse(row["DepartmentId"]?.ToString(), out var deptId) ? deptId : (int?)null,
                JobAssignmentId = row["JobAssignmentId"] != DBNull.Value ? Convert.ToInt32(row["JobAssignmentId"]) : 0,
                Status = Convert.ToInt32(row["EmploymentStatusId"]),
                Password = row["Password"]?.ToString(),
                PasswordSalt = (byte[])row["PasswordSalt"],
                FirstName = row["FirstName"]?.ToString(),
                LastName = row["LastName"]?.ToString(),
                MiddleInitial = row["MiddleInitial"] != DBNull.Value ? Convert.ToChar(row["MiddleInitial"]) : (char?)null,
                StreetAddress = row["StreetAddress"]?.ToString(),
                City = row["City"]?.ToString(),
                Province = row["Province"]?.ToString(),
                PostalCode = row["PostalCode"]?.ToString(),
                DateOfBirth = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]) : DateTime.MinValue,
                SIN = row["SIN"]?.ToString(),
                SeniorityDate = row["SeniorityDate"] != DBNull.Value ? Convert.ToDateTime(row["SeniorityDate"]) : DateTime.MinValue,
                JobStartDate = row["JobStartDate"] != DBNull.Value ? Convert.ToDateTime(row["JobStartDate"]) : DateTime.MinValue,
                WorkPhone = row["WorkPhone"]?.ToString(),
                CellPhone = row["CellPhone"]?.ToString(),
                EmailAddress = row["EmailAddress"]?.ToString(),
                OfficeLocation = row["OfficeLocation"]?.ToString(),
                RetirementDate = row["RetirementDate"] != DBNull.Value ? Convert.ToDateTime(row["RetirementDate"]) : null,
                TerminationDate = row["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(row["TerminationDate"]) : null,
                RecordVersion = row["RecordVersion"] != DBNull.Value ? (byte[])row["RecordVersion"] : null
            };
        }

        public AccessLevels GetEmployeeAccessLevel(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = _db.Execute("spGetEmployeeJob", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string title = row["Title"].ToString()!;

                if (title == "CEO")
                {
                    return AccessLevels.CEO;
                }
                else if (title == "HR Manager")
                {
                    return AccessLevels.HRSupervisor;
                }
                else if (
                    title == "Project Manager" ||
                    title == "Product Manager" ||
                    title == "Developer Manager" ||
                    title == "Infrastructure Manager")
                {
                    return AccessLevels.RegularSupervisor;
                }
                else if (title == "HR Officer")
                {
                    return AccessLevels.HREmployee;
                }
                else if (
                    title == "Systems Analyst" ||
                    title == "Solution Architect" ||
                    title == "Software Engineer" ||
                    title == "Network Engineer" ||
                    title == "Database Administrator" ||
                    title == "Cybersecurity Specialist" ||
                    title == "QA Engineer" ||
                    title == "Technical Support Engineer" ||
                    title == "Business Analyst" ||
                    title == "DevOps Engineer" ||
                    title == "UI/UX Designer")
                {
                    return AccessLevels.RegularEmployee;
                }
            }
            throw new Exception($"Something went wrong while getting access level");
        }

        public AccessLevels GetEmployeeAccessLevelByJobTitle(string title)
        {
            if (title == "CEO")
            {
                return AccessLevels.CEO;
            }
            else if (title == "HR Manager")
            {
                return AccessLevels.HRSupervisor;
            }
            else if (
                title == "Project Manager" ||
                title == "Product Manager" ||
                title == "Developer Manager" ||
                title == "Infrastructure Manager")
            {
                return AccessLevels.RegularSupervisor;
            }
            else if (title == "HR Officer")
            {
                return AccessLevels.HREmployee;
            }
            else if (
                title == "Systems Analyst" ||
                title == "Solution Architect" ||
                title == "Software Engineer" ||
                title == "Network Engineer" ||
                title == "Database Administrator" ||
                title == "Cybersecurity Specialist" ||
                title == "QA Engineer" ||
                title == "Technical Support Engineer" ||
                title == "Business Analyst" ||
                title == "DevOps Engineer" ||
                title == "UI/UX Designer")
            {
                return AccessLevels.RegularEmployee;
            }

            throw new Exception($"Something went wrong while getting access level");
        }

        /// <summary>
        /// Creates an EmployeeDTO by mapping values from a DataRow.
        /// </summary>
        /// <param name="row">The DataRow containing employee data.</param>
        /// <returns>A populated EmployeeDTO with values from the DataRow.</returns>
        private EmployeeDTO PopulateEmployeeDTOFromDataRow(DataRow row)
        {
            return new EmployeeDTO
            {
                Id = row["Id"].ToString(),
                SupervisorId = row["SupervisorId"].ToString(),
                DepartmentId = int.TryParse(row["DepartmentId"]?.ToString(), out var deptId) ? deptId : (int?)null,
                JobAssignmentId = Convert.ToInt32(row["JobAssignmentId"]),
                Status = Convert.ToInt32(row["EmploymentStatusId"]),
                Password = row["Password"].ToString(),
                PasswordSalt = (byte[])row["PasswordSalt"],
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
                RetirementDate = row["RetirementDate"] != DBNull.Value ? Convert.ToDateTime(row["RetirementDate"]) : null,
                TerminationDate = row["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(row["TerminationDate"]) : null,
                OfficeLocation = row["OfficeLocation"].ToString()
            };
        }

        /// <summary>
        /// Populates and returns a <see cref="SupervisorsDTO"/> object using the data from the specified <see cref="DataRow"/>.
        /// </summary>
        /// <param name="row">The <see cref="DataRow"/> containing supervisor data.</param>
        /// <returns>A populated <see cref="SupervisorsDTO"/> instance based on the provided data row.</returns>

        private SupervisorsDTO PopulateSuperVisorsDTOFromDataRow(DataRow row)
        {
            return new SupervisorsDTO
            {
                Id = row["Id"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                JobTitle = row["JobTitle"].ToString(),
                SupervisedEmployeeCount = Convert.ToInt32(row["SupervisedCount"])
            };
        }

        /// <summary>
        /// Creates and returns an <see cref="Employee"/> entity by mapping data from the provided <see cref="EmployeeDTO"/>.
        /// </summary>
        /// <param name="employeeDTO">The data transfer object containing employee information.</param>
        /// <returns>An <see cref="Employee"/> entity populated with data from the DTO.</returns>

        private Employee PopulateEmployee(EmployeeDTO employeeDTO)
        {
            return new Employee
            {
                Id = employeeDTO.Id,
                SupervisorId = employeeDTO.SupervisorId,
                DepartmentId = employeeDTO.DepartmentId,
                JobAssignmentId = employeeDTO.JobAssignmentId,
                Password = employeeDTO.Password,
                PasswordSalt = employeeDTO.PasswordSalt,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                MiddleInitial = employeeDTO.MiddleInitial,
                StreetAddress = employeeDTO.StreetAddress,
                City = employeeDTO.City,
                Province = employeeDTO.Province,
                PostalCode = employeeDTO.PostalCode,
                DateOfBirth = employeeDTO.DateOfBirth,
                SIN = employeeDTO.SIN,
                SeniorityDate = employeeDTO.SeniorityDate,
                JobStartDate = employeeDTO.JobStartDate,
                WorkPhone = employeeDTO.WorkPhone,
                CellPhone = employeeDTO.CellPhone,
                EmailAddress = employeeDTO.EmailAddress,
                OfficeLocation = employeeDTO.OfficeLocation,
                Status = employeeDTO.Status,
                RetirementDate = employeeDTO.RetirementDate,
                TerminationDate = employeeDTO.TerminationDate
            };
        }

        private EmployeeDTO PopulateEmployeeDTO(Employee emp)
        {
            return new EmployeeDTO
            {
                Id = emp.Id,
                SupervisorId = emp.SupervisorId,
                DepartmentId = emp.DepartmentId,
                JobAssignmentId = emp.JobAssignmentId,
                Password = emp.Password,
                PasswordSalt = emp.PasswordSalt,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MiddleInitial = emp.MiddleInitial,
                StreetAddress = emp.StreetAddress,
                City = emp.City,
                Province = emp.Province,
                PostalCode = emp.PostalCode,
                DateOfBirth = emp.DateOfBirth,
                SIN = emp.SIN,
                SeniorityDate = emp.SeniorityDate,
                JobStartDate = emp.JobStartDate,
                WorkPhone = emp.WorkPhone,
                CellPhone = emp.CellPhone,
                EmailAddress = emp.EmailAddress,
                OfficeLocation = emp.OfficeLocation,
                Status = emp.Status,
                TerminationDate = emp.TerminationDate,
                RetirementDate = emp.RetirementDate
            };

        }

        public async Task<AccessLevels> GetEmployeeAccessLevelAsync(string id)
        {
            List<Parm> parms = new()
            {
                new("@Id", SqlDbType.Char, id, 8)
            };

            DataTable dt = await _db.ExecuteAsync("spGetEmployeeJob", parms);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string title = row["Title"].ToString()!;

                if (title == "CEO")
                {
                    return AccessLevels.CEO;
                }
                else if (title == "HR Manager")
                {
                    return AccessLevels.HRSupervisor;
                }
                else if (
                    title == "Project Manager" ||
                    title == "Product Manager" ||
                    title == "Developer Manager" ||
                    title == "Infrastructure Manager")
                {
                    return AccessLevels.RegularSupervisor;
                }
                else if (title == "HR Officer")
                {
                    return AccessLevels.HREmployee;
                }
                else if (
                    title == "Systems Analyst" ||
                    title == "Solution Architect" ||
                    title == "Software Engineer" ||
                    title == "Network Engineer" ||
                    title == "Database Administrator" ||
                    title == "Cybersecurity Specialist" ||
                    title == "QA Engineer" ||
                    title == "Technical Support Engineer" ||
                    title == "Business Analyst" ||
                    title == "DevOps Engineer" ||
                    title == "UI/UX Designer")
                {
                    return AccessLevels.RegularEmployee;
                }
            }
            throw new Exception($"Something went wrong while getting access level");
        }

        public List<SupervisorsDTO> GetAvailableSuperVisors()
        {
            DataTable dt = _db.Execute("spGetSupervisorsWithFewerThan10");


            return dt.AsEnumerable().Select(row => PopulateSuperVisorsDTOFromDataRow(row)).ToList();
        }


        public async Task<List<SupervisorsDTO>> GetAvailableSuperVisorsAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetSupervisorsWithFewerThan10");


            return dt.AsEnumerable().Select(row => PopulateSuperVisorsDTOFromDataRow(row)).ToList();
        }

        public bool IsUniqueSIN(Employee emp)
        {
            List<Parm> parms = new()
            {
                new("@SIN", SqlDbType.VarChar, emp.SIN),
                new("@ExcludeId", SqlDbType.Char, emp.Id)
            };

            DataTable dt = _db.Execute("spGetEmployeeBySIN", parms);

            return dt.Rows.Count == 0;
        }

        public List<EmployeeListDTO> GetEmployeesListDisplay()
        {
            DataTable dt = _db.Execute("spGetAllEmployeesDisplay");

            return dt.AsEnumerable().Select(row => new EmployeeListDTO
            {
                Id = row["Id"].ToString(),
                FullName = row["FullName"].ToString(),
                Supervisor = row["Supervisor"].ToString(),
                Department = row["Department"].ToString(),
                Job = row["Job"].ToString(),
                OfficeLocation = row["OfficeLocation"].ToString()
            }).ToList();
        }

        public async Task<List<EmployeeListDTO>> GetEmployeesListDisplayAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllEmployeesDisplay");

            return dt.AsEnumerable().Select(row => new EmployeeListDTO
            {
                Id = row["Id"].ToString(),
                FullName = row["FullName"].ToString(),
                Supervisor = row["Supervisor"].ToString(),
                Department = row["Department"].ToString(),
                Job = row["Job"].ToString(),
                OfficeLocation = row["OfficeLocation"].ToString()
            }).ToList();
        }

        public List<SupervisorsDTO> GetAvailableSuperVisorsByDepartment(int deptId)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, deptId)
            };

            DataTable dt = _db.Execute("GetSupervisorsByDepartment", parms);


            return dt.AsEnumerable().Select(row => PopulateSuperVisorsDTOFromDataRow(row)).ToList();
        }

        public async Task<List<SupervisorsDTO>> GetAvailableSuperVisorsByDepartmentAsync(int deptId)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, deptId)
            };

            DataTable dt = await _db.ExecuteAsync("GetSupervisorsByDepartment", parms);


            return dt.AsEnumerable().Select(row => PopulateSuperVisorsDTOFromDataRow(row)).ToList();
        }

        public List<EmployeeSearchResultDetailedDTO?> SearchEmployeeDetailed(EmployeeSearchDTO searchDto)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, searchDto.DepartmentId),
                new("@EmployeeID", SqlDbType.Char, searchDto.EmployeeId),
                new("@LastName", SqlDbType.VarChar, searchDto.LastName)
            };

            DataTable dt = _db.Execute("spSearchEmployee", parms);

            return dt.AsEnumerable().Select(row => new EmployeeSearchResultDetailedDTO
            {
                FirstName = row["FirstName"].ToString(),
                MiddleInitial = row["MiddleInitial"].ToString(),
                LastName = row["LastName"].ToString(),
                HomeAddress = row["HomeAddress"].ToString(),
                WorkPhone = row["WorkPhone"].ToString(),
                CellPhone = row["CellPhone"].ToString(),
                WorkEmail = row["EmailAddress"].ToString()
            }).ToList()!;
        }

        public async Task<List<EmployeeSearchResultDetailedDTO?>> SearchEmployeeDetailedAsync(EmployeeSearchDTO searchDto)
        {
            List<Parm> parms = new()
            {
                new("@DepartmentId", SqlDbType.Int, searchDto.DepartmentId),
                new("@EmployeeID", SqlDbType.Char, searchDto.EmployeeId),
                new("@LastName", SqlDbType.VarChar, searchDto.LastName)
            };

            DataTable dt = await _db.ExecuteAsync("spSearchEmployee", parms);

            return dt.AsEnumerable().Select(row => new EmployeeSearchResultDetailedDTO
            {
                FirstName = row["FirstName"].ToString(),
                MiddleInitial = row["MiddleInitial"].ToString(),
                LastName = row["LastName"].ToString(),
                HomeAddress = row["HomeAddress"].ToString(),
                WorkPhone = row["WorkPhone"].ToString(),
                CellPhone = row["CellPhone"].ToString(),
                WorkEmail = row["EmailAddress"].ToString()
            }).ToList()!;
        }
    }
}

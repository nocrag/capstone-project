using MJRecords.Model;
using MJRecords.Repository;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRepo _repo;
        private readonly IJobService _jobService;

        public EmployeeService(IEmployeeRepo repo, IJobService jobService)
        {
            _repo = repo;
            _jobService = jobService;
        }

        public async Task<List<EmployeeDTO>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<EmployeeDTO> GetAsync(string id)
        {
            return await _repo.GetAsync(id);
        }

        public async Task<Employee> CreateAsync(EmployeeDTO emp)
        {
            Employee employee = EmployeeConverter(emp);

            Validate(employee, false);

            if (employee.Errors.Count == 0)
            {
                emp.PasswordSalt = PasswordUtility.GenerateSalt();
                emp.Password = PasswordUtility.HashToSHA256(emp.Password!, emp.PasswordSalt);
                await _repo.CreateAsync(emp);
            }

            return employee;
        }

        public async Task<Employee> DeleteAsync(Employee emp)
        {
            return await _repo.DeleteAsync(emp);
        }

        public async Task<Employee> UpdateAsync(Employee emp)
        {
            Employee dbEmp = _repo.GetEmp(emp.Id);

            // Case 1: Password is provided - generate new salt and hash
            if (emp.Password != null)
            {
                byte[] newSalt = PasswordUtility.GenerateSalt();
                emp.PasswordSalt = newSalt;
                emp.Password = PasswordUtility.HashToSHA256(emp.Password, newSalt);

                Validate(emp, false);
            }
            // Case 2: No password provided - use existing password from DB
            else
            {
                emp.Password = dbEmp.Password;
                emp.PasswordSalt = dbEmp.PasswordSalt;

                Validate(emp, true);
            }

            return await _repo.UpdateAsync(emp);
        }

        public List<EmployeeDTO> GetAll()
        {
            return _repo.GetAll();
        }

        public EmployeeDTO Get(string id)
        {
            return _repo.Get(id);
        }

        public Employee Create(EmployeeDTO emp)
        {
            Employee employee = EmployeeConverter(emp);

            Validate(employee, false);

            if(employee.Errors.Count == 0)
            {
                emp.PasswordSalt = PasswordUtility.GenerateSalt();
                emp.Password = PasswordUtility.HashToSHA256(emp.Password!, emp.PasswordSalt);
                return _repo.Create(emp);
            }

            return employee;
        }

        public Employee Update(Employee emp)
        {
            Employee dbEmp = _repo.GetEmp(emp.Id);

            // Case 1: Password is provided - generate new salt and hash
            if (emp.Password != null)
            {
                // Store original password for validation
                string originalPassword = emp.Password;

                // Validate with the original unhashed password
                Validate(emp, false);

                // If validation passes, then hash the password
                if (emp.Errors.Count == 0)
                {
                    byte[] newSalt = PasswordUtility.GenerateSalt();
                    emp.PasswordSalt = newSalt;
                    emp.Password = PasswordUtility.HashToSHA256(originalPassword, newSalt);
                }
            }
            // Case 2: No password provided - use existing password from DB
            else
            {
                emp.Password = dbEmp.Password;
                emp.PasswordSalt = dbEmp.PasswordSalt;

                Validate(emp, true);
            }

            
            if (emp.Errors.Count == 0)
            {

                return _repo.Update(emp);
            }

            return emp;
        }

        public Employee Delete(Employee emp)
        {
            return _repo.Delete(emp);
        }

        public List<EmployeeListDTO> GetEmployeesListDisplay()
        {
            return _repo.GetEmployeesListDisplay();
        }

        public async Task<List<EmployeeListDTO>> GetEmployeesListDisplayAsync()
        {
            return await _repo.GetEmployeesListDisplayAsync();
        }

        public void Validate(Employee emp, bool skipPassCheck)
        {
            // 1. Validate data annotations
            List<ValidationResult> results = new();
            Validator.TryValidateObject(emp, new ValidationContext(emp), results, true);


            Job job = _jobService.Get(emp.JobAssignmentId);
            AccessLevels accessLevel = GetEmployeeAccessLevelByJobTitle(job.Title!);

            // check here
            foreach (ValidationResult e in results)
            {
                if (skipPassCheck && e.MemberNames.Contains("Password"))
                {
                    continue;
                }
                emp.AddError(new(e.ErrorMessage, ErrorType.Model));
            }

            // 1.5 Check For Unique SIN
            if (!IsUniqueSIN(emp))
            {
                emp.AddError(new("Please use a unique Social Insurance Number.", ErrorType.Business));
                //Employee empDb = _repo.GetEmp(emp.Id);
                //if (!emp.Id.Equals(empDb.Id))
                //{
                //    emp.AddError(new("Please use a unique Social Insurance Number.", ErrorType.Business));
                //}
            }

            // 2. Check legal age
            if (!IsLegalAge(emp))
            {
                emp.AddError(new("Employee must be 16 years of age or greater.", ErrorType.Business));
            }

            // 3. Supervisor & Department Validation
            if (accessLevel != AccessLevels.CEO)
            {
                // Supervisor is required for all non-CEOs
                if (emp.SupervisorId == null)
                {
                    emp.AddError(new("Supervisor is required for this role.", ErrorType.Business));
                }
                else
                {
                    // Fetch supervisor
                    Employee supervisor = EmployeeConverter(_repo.Get(emp.SupervisorId!));
                    if (supervisor == null)
                    {
                        emp.AddError(new("Assigned supervisor does not exist.", ErrorType.Business));
                    }
                    else
                    {
                        // Check supervisor role
                        Job supervisorJob = _jobService.Get(supervisor.JobAssignmentId);
                        bool supervisorIsCEO = supervisorJob.Title == "CEO";

                        // Supervisor must be in same department unless they're the CEO
                        if (!supervisorIsCEO && supervisor.DepartmentId != emp.DepartmentId)
                        {
                            emp.AddError(new("Supervisor must be in the same department unless they are the CEO.", ErrorType.Business));
                        }
                    }
                }

                // Department is also required for non-CEO roles
                if (emp.DepartmentId == null)
                {
                    emp.AddError(new("Department assignment is required for this role.", ErrorType.Business));
                }
            }


        }

        public AccessLevels GetEmployeeAccessLevel(string id)
        {
            return _repo.GetEmployeeAccessLevel(id);
        }
        public AccessLevels GetEmployeeAccessLevelByJobTitle(string title)
        {
            return _repo.GetEmployeeAccessLevelByJobTitle(title);
        }

        public bool IsLegalAge(Employee emp)
        {
            var minimumLegalAge = DateTime.Today.AddYears(-16);
            return emp.DateOfBirth <= minimumLegalAge;
        }

        public Employee EmployeeConverter(EmployeeDTO empDto)
        {
            return new Employee
            {
                Id = empDto.Id,
                SupervisorId = empDto.SupervisorId,
                DepartmentId = empDto.DepartmentId,
                JobAssignmentId = empDto.JobAssignmentId,
                Status = empDto.Status,
                Password = empDto.Password,
                PasswordSalt = empDto.PasswordSalt,
                FirstName = empDto.FirstName,
                LastName = empDto.LastName,
                MiddleInitial = empDto.MiddleInitial,
                StreetAddress = empDto.StreetAddress,
                City = empDto.City,
                Province = empDto.Province,
                PostalCode = empDto.PostalCode,
                DateOfBirth = empDto.DateOfBirth,
                SIN = empDto.SIN,
                SeniorityDate = empDto.SeniorityDate,
                JobStartDate = empDto.JobStartDate,
                WorkPhone = empDto.WorkPhone,
                CellPhone = empDto.CellPhone,
                EmailAddress = empDto.EmailAddress,
                RetirementDate = empDto.RetirementDate,
                TerminationDate = empDto.TerminationDate,
                OfficeLocation = empDto.OfficeLocation
            };
        }

        public EmployeeDTO EmployeeConverter(Employee emp)
        {
            return new EmployeeDTO
            {
                Id = emp.Id,
                SupervisorId = emp.SupervisorId,
                DepartmentId = emp.DepartmentId,
                JobAssignmentId = emp.JobAssignmentId,
                Status = emp.Status,
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
                RetirementDate = emp.RetirementDate,
                TerminationDate = emp.TerminationDate,
                OfficeLocation = emp.OfficeLocation
            };
        }

        public EmployeeDetailsDto? GetEmployeeDetails(string employeeId)
        {
            return _repo.GetEmployeeDetails(employeeId);
        }

        public async Task<EmployeeDetailsDto?> GetEmployeeDetailsAsync(string employeeId)
        {
            return await _repo.GetEmployeeDetailsAsync(employeeId);
        }

        public List<SupervisorsDTO> GetAvailableSuperVisors()
        {
            return _repo.GetAvailableSuperVisors();
        }

        public async Task<List<SupervisorsDTO>> GetAvailableSuperVisorsAsync()
        {
            return await _repo.GetAvailableSuperVisorsAsync();
        }

        public bool IsUniqueSIN(Employee emp)
        {
            return _repo.IsUniqueSIN(emp);
        }

        public List<EmployeeSearchResultDTO?> SearchEmployee(EmployeeSearchDTO parms)
        {
            return _repo.SearchEmployee(parms);
        }

        public async Task<List<EmployeeSearchResultDTO?>> SearchEmployeeAsync(EmployeeSearchDTO parms)
        {
            return await _repo.SearchEmployeeAsync(parms);
        }

        public List<SupervisorsDTO> GetAvailableSuperVisorsByDepartment(int deptId)
        {
            return _repo.GetAvailableSuperVisorsByDepartment(deptId);
        }

        public async Task<List<SupervisorsDTO>> GetAvailableSuperVisorsByDepartmentAsync(int deptId)
        {
            return await _repo.GetAvailableSuperVisorsByDepartmentAsync(deptId);
        }

        public List<EmployeeSearchResultDetailedDTO?> SearchEmployeeDetailed(EmployeeSearchDTO parms)
        {
            return _repo.SearchEmployeeDetailed(parms);
        }

        public async Task<List<EmployeeSearchResultDetailedDTO?>> SearchEmployeeDetailedAsync(EmployeeSearchDTO parms)
        {
            return await _repo.SearchEmployeeDetailedAsync(parms);
        }

        public Employee GetEmp(string id)
        {
            return _repo.GetEmp(id);
        }

        public async Task<Employee> GetEmpAsync(string id)
        {
            return await _repo.GetEmpAsync(id);
        }
    }
}

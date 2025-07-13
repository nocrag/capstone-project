using MJRecords.Model;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of <see cref="EmployeeDTO"/> objects.</returns>
        List<EmployeeDTO> GetAll();

        /// <summary>
        /// Retrieves all available supervisors across departments.
        /// </summary>
        /// <returns>A list of <see cref="SupervisorsDTO"/> objects.</returns>
        List<SupervisorsDTO> GetAvailableSuperVisors();

        /// <summary>
        /// Retrieves available supervisors for a specific department.
        /// </summary>
        /// <param name="deptId">The ID of the department.</param>
        /// <returns>A list of <see cref="SupervisorsDTO"/> objects.</returns>
        List<SupervisorsDTO> GetAvailableSuperVisorsByDepartment(int deptId);

        /// <summary>
        /// Retrieves a specific employee by their ID as a DTO.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The corresponding <see cref="EmployeeDTO"/>.</returns>
        EmployeeDTO Get(string id);

        /// <summary>
        /// Retrieves a specific employee entity by their ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The corresponding <see cref="Employee"/>.</returns>
        Employee GetEmp(string id);

        /// <summary>
        /// Creates a new employee from a DTO.
        /// </summary>
        /// <param name="emp">The employee DTO.</param>
        /// <returns>The created <see cref="Employee"/> entity.</returns>
        Employee Create(EmployeeDTO emp);

        /// <summary>
        /// Updates an existing employee entity.
        /// </summary>
        /// <param name="emp">The employee entity with updated data.</param>
        /// <returns>The updated <see cref="Employee"/>.</returns>
        Employee Update(Employee emp);

        /// <summary>
        /// Deletes an employee entity.
        /// </summary>
        /// <param name="emp">The employee entity to delete.</param>
        /// <returns>The deleted <see cref="Employee"/>.</returns>
        Employee Delete(Employee emp);

        /// <summary>
        /// Retrieves a simplified display list of employees.
        /// </summary>
        /// <returns>A list of <see cref="EmployeeListDTO"/> objects.</returns>
        List<EmployeeListDTO> GetEmployeesListDisplay();

        /// <summary>
        /// Retrieves the access level of an employee by their ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The corresponding <see cref="AccessLevels"/>.</returns>
        AccessLevels GetEmployeeAccessLevel(string id);

        /// <summary>
        /// Retrieves the access level for an employee based on their job title.
        /// </summary>
        /// <param name="title">The job title.</param>
        /// <returns>The corresponding <see cref="AccessLevels"/>.</returns>
        AccessLevels GetEmployeeAccessLevelByJobTitle(string title);

        /// <summary>
        /// Converts an <see cref="EmployeeDTO"/> to an <see cref="Employee"/> entity.
        /// </summary>
        /// <param name="empDto">The employee DTO.</param>
        /// <returns>The corresponding <see cref="Employee"/>.</returns>
        Employee EmployeeConverter(EmployeeDTO empDto);

        /// <summary>
        /// Converts an <see cref="Employee"/> entity to an <see cref="EmployeeDTO"/>.
        /// </summary>
        /// <param name="emp">The employee entity.</param>
        /// <returns>The corresponding <see cref="EmployeeDTO"/>.</returns>
        EmployeeDTO EmployeeConverter(Employee emp);

        /// <summary>
        /// Retrieves detailed information for a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee ID.</param>
        /// <returns>The corresponding <see cref="EmployeeDetailsDto"/>, or null if not found.</returns>
        EmployeeDetailsDto? GetEmployeeDetails(string employeeId);

        /// <summary>
        /// Searches for employees matching the specified parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A list of <see cref="EmployeeSearchResultDTO"/> objects, possibly containing nulls.</returns>
        List<EmployeeSearchResultDTO?> SearchEmployee(EmployeeSearchDTO parms);

        /// <summary>
        /// Performs a detailed search for employees matching the specified parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A list of <see cref="EmployeeSearchResultDetailedDTO"/> objects, possibly containing nulls.</returns>
        List<EmployeeSearchResultDetailedDTO?> SearchEmployeeDetailed(EmployeeSearchDTO parms);

        /// <summary>
        /// Validates an employee entity, optionally skipping password validation.
        /// </summary>
        /// <param name="emp">The employee to validate.</param>
        /// <param name="skipPassCheck">Whether to skip password checks during validation.</param>
        void Validate(Employee emp, bool skipPassCheck);

        /// <summary>
        /// Determines whether an employee meets the legal age requirement.
        /// </summary>
        /// <param name="emp">The employee entity.</param>
        /// <returns><c>true</c> if the employee is of legal age; otherwise, <c>false</c>.</returns>
        bool IsLegalAge(Employee emp);

        /// <summary>
        /// Checks if the employee's SIN is unique.
        /// </summary>
        /// <param name="emp">The employee entity.</param>
        /// <returns><c>true</c> if the SIN is unique; otherwise, <c>false</c>.</returns>
        bool IsUniqueSIN(Employee emp);

        /// <summary>
        /// Asynchronously retrieves all employees.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeDTO"/> objects.</returns>
        Task<List<EmployeeDTO>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a specific employee entity by their ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="Employee"/>.</returns>
        Task<Employee> GetEmpAsync(string id);

        /// <summary>
        /// Asynchronously retrieves a specific employee by their ID as a DTO.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="EmployeeDTO"/>.</returns>
        Task<EmployeeDTO> GetAsync(string id);

        /// <summary>
        /// Asynchronously creates a new employee from a DTO.
        /// </summary>
        /// <param name="emp">The employee DTO.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="Employee"/> entity.</returns>
        Task<Employee> CreateAsync(EmployeeDTO emp);

        /// <summary>
        /// Asynchronously updates an existing employee entity.
        /// </summary>
        /// <param name="emp">The employee entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Employee"/>.</returns>
        Task<Employee> UpdateAsync(Employee emp);

        /// <summary>
        /// Asynchronously deletes an employee entity.
        /// </summary>
        /// <param name="emp">The employee entity to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deleted <see cref="Employee"/>.</returns>
        Task<Employee> DeleteAsync(Employee emp);

        /// <summary>
        /// Asynchronously retrieves detailed information for a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee ID.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="EmployeeDetailsDto"/>, or null if not found.</returns>
        Task<EmployeeDetailsDto?> GetEmployeeDetailsAsync(string employeeId);

        /// <summary>
        /// Asynchronously retrieves all available supervisors across departments.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="SupervisorsDTO"/> objects.</returns>
        Task<List<SupervisorsDTO>> GetAvailableSuperVisorsAsync();

        /// <summary>
        /// Asynchronously retrieves available supervisors for a specific department.
        /// </summary>
        /// <param name="deptId">The ID of the department.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="SupervisorsDTO"/> objects.</returns>
        Task<List<SupervisorsDTO>> GetAvailableSuperVisorsByDepartmentAsync(int deptId);

        /// <summary>
        /// Asynchronously retrieves a simplified display list of employees.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeListDTO"/> objects.</returns>
        Task<List<EmployeeListDTO>> GetEmployeesListDisplayAsync();

        /// <summary>
        /// Asynchronously searches for employees matching the specified parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeSearchResultDTO"/> objects, possibly containing nulls.</returns>
        Task<List<EmployeeSearchResultDTO?>> SearchEmployeeAsync(EmployeeSearchDTO parms);

        /// <summary>
        /// Asynchronously performs a detailed search for employees matching the specified parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeSearchResultDetailedDTO"/> objects, possibly containing nulls.</returns>
        Task<List<EmployeeSearchResultDetailedDTO?>> SearchEmployeeDetailedAsync(EmployeeSearchDTO parms);

    }
}

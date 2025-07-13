using MJRecords.Model;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IEmployeeRepo
    {
        /// <summary>
        /// Gets all employees as DTOs.
        /// </summary>
        /// <returns>A list of employee DTOs.</returns>
        List<EmployeeDTO> GetAll();

        /// <summary>
        /// Gets all available supervisors.
        /// </summary>
        /// <returns>A list of supervisor DTOs.</returns>
        List<SupervisorsDTO> GetAvailableSuperVisors();

        /// <summary>
        /// Gets all available supervisors for a specific department.
        /// </summary>
        /// <param name="deptId">The department ID.</param>
        /// <returns>A list of supervisor DTOs for the specified department.</returns>
        List<SupervisorsDTO> GetAvailableSuperVisorsByDepartment(int deptId);

        /// <summary>
        /// Gets an employee DTO by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The employee DTO if found; otherwise, null.</returns>
        EmployeeDTO Get(string id);

        /// <summary>
        /// Gets an employee entity by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The employee entity if found; otherwise, null.</returns>
        Employee GetEmp(string id);

        /// <summary>
        /// Creates a new employee from a DTO.
        /// </summary>
        /// <param name="emp">The employee DTO containing the information for the new employee.</param>
        /// <returns>The created employee entity with updated information.</returns>
        Employee Create(EmployeeDTO emp);

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="emp">The employee with updated information.</param>
        /// <returns>The updated employee entity.</returns>
        Employee Update(Employee emp);

        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// <param name="emp">The employee to delete.</param>
        /// <returns>The deleted employee entity.</returns>
        Employee Delete(Employee emp);

        /// <summary>
        /// Gets detailed information for a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee ID.</param>
        /// <returns>The employee details DTO if found; otherwise, null.</returns>
        EmployeeDetailsDto? GetEmployeeDetails(string employeeId);

        /// <summary>
        /// Gets a list of employees formatted for display.
        /// </summary>
        /// <returns>A list of employee list DTOs.</returns>
        List<EmployeeListDTO> GetEmployeesListDisplay();

        /// <summary>
        /// Searches for employees based on search parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A list of employee search result DTOs matching the search criteria.</returns>
        List<EmployeeSearchResultDTO?> SearchEmployee(EmployeeSearchDTO parms);

        /// <summary>
        /// Searches for employees with detailed information based on search parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A list of detailed employee search result DTOs matching the search criteria.</returns>
        List<EmployeeSearchResultDetailedDTO?> SearchEmployeeDetailed(EmployeeSearchDTO parms);

        /// <summary>
        /// Gets the access level for an employee.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The access level of the employee.</returns>
        AccessLevels GetEmployeeAccessLevel(string id);

        /// <summary>
        /// Gets the access level based on job title.
        /// </summary>
        /// <param name="title">The job title.</param>
        /// <returns>The access level associated with the job title.</returns>
        AccessLevels GetEmployeeAccessLevelByJobTitle(string title);

        /// <summary>
        /// Checks if the employee's Social Insurance Number (SIN) is unique.
        /// </summary>
        /// <param name="emp">The employee to check.</param>
        /// <returns>True if the SIN is unique; otherwise, false.</returns>
        bool IsUniqueSIN(Employee emp);

        /// <summary>
        /// Asynchronously gets an employee entity by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the employee entity if found; otherwise, null.</returns>
        Task<Employee> GetEmpAsync(string id);

        /// <summary>
        /// Asynchronously gets all employees as DTOs.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of employee DTOs.</returns>
        Task<List<EmployeeDTO>> GetAllAsync();

        /// <summary>
        /// Asynchronously gets an employee DTO by ID.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the employee DTO if found; otherwise, null.</returns>
        Task<EmployeeDTO> GetAsync(string id);

        /// <summary>
        /// Asynchronously creates a new employee from a DTO.
        /// </summary>
        /// <param name="emp">The employee DTO containing the information for the new employee.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created employee entity with updated information.</returns>
        Task<Employee> CreateAsync(EmployeeDTO emp);

        /// <summary>
        /// Asynchronously updates an existing employee.
        /// </summary>
        /// <param name="emp">The employee with updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated employee entity.</returns>
        Task<Employee> UpdateAsync(Employee emp);

        /// <summary>
        /// Asynchronously deletes an employee.
        /// </summary>
        /// <param name="emp">The employee to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted employee entity.</returns>
        Task<Employee> DeleteAsync(Employee emp);

        /// <summary>
        /// Asynchronously gets detailed information for a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the employee details DTO if found; otherwise, null.</returns>
        Task<EmployeeDetailsDto?> GetEmployeeDetailsAsync(string employeeId);

        /// <summary>
        /// Asynchronously gets the access level for an employee.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access level of the employee.</returns>
        Task<AccessLevels> GetEmployeeAccessLevelAsync(string id);

        /// <summary>
        /// Asynchronously gets all available supervisors.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of supervisor DTOs.</returns>
        Task<List<SupervisorsDTO>> GetAvailableSuperVisorsAsync();

        /// <summary>
        /// Asynchronously gets all available supervisors for a specific department.
        /// </summary>
        /// <param name="deptId">The department ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of supervisor DTOs for the specified department.</returns>
        Task<List<SupervisorsDTO>> GetAvailableSuperVisorsByDepartmentAsync(int deptId);

        /// <summary>
        /// Asynchronously gets a list of employees formatted for display.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of employee list DTOs.</returns>
        Task<List<EmployeeListDTO>> GetEmployeesListDisplayAsync();

        /// <summary>
        /// Asynchronously searches for employees based on search parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of employee search result DTOs matching the search criteria.</returns>
        Task<List<EmployeeSearchResultDTO?>> SearchEmployeeAsync(EmployeeSearchDTO parms);

        /// <summary>
        /// Asynchronously searches for employees with detailed information based on search parameters.
        /// </summary>
        /// <param name="parms">The search parameters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of detailed employee search result DTOs matching the search criteria.</returns>
        Task<List<EmployeeSearchResultDetailedDTO?>> SearchEmployeeDetailedAsync(EmployeeSearchDTO parms);
    }
}

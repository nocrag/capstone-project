using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IDepartmentRepo
    {
        /// <summary>
        /// Gets all departments as DTOs.
        /// </summary>
        /// <returns>A list of department DTOs.</returns>
        List<DepartmentDTO> GetAll();

        /// <summary>
        /// Gets a department by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>The department if found; otherwise, null.</returns>
        Department GetDept(int id);

        /// <summary>
        /// Gets a department DTO by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>The department DTO if found; otherwise, null.</returns>
        DepartmentDTO Get(int id);

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="dept">The department to create.</param>
        /// <returns>The created department with updated information.</returns>
        Department Create(Department dept);

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="dept">The department with updated information.</param>
        /// <returns>The updated department.</returns>
        Department Update(Department dept);

        /// <summary>
        /// Deletes a department.
        /// </summary>
        /// <param name="dept">The department to delete.</param>
        /// <returns>The deleted department.</returns>
        Department Delete(Department dept);

        /// <summary>
        /// Converts a department DTO to a department entity.
        /// </summary>
        /// <param name="deptDto">The department DTO to convert.</param>
        /// <returns>A department entity populated with the DTO data.</returns>
        Department PopulateDepartmentFromDTO(DepartmentDTO deptDto);

        /// <summary>
        /// Checks if a department name is unique.
        /// </summary>
        /// <param name="name">The department name to check.</param>
        /// <returns>True if the name is unique; otherwise, false.</returns>
        bool IsDepartmentNameUnique(string name);

        // ASYNC METHODS

        /// <summary>
        /// Asynchronously gets all departments as DTOs.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of department DTOs.</returns>
        Task<List<DepartmentDTO>> GetAllAsync();

        /// <summary>
        /// Asynchronously gets a department DTO by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the department DTO if found; otherwise, null.</returns>
        Task<DepartmentDTO> GetAsync(int id);

        /// <summary>
        /// Asynchronously creates a new department.
        /// </summary>
        /// <param name="dept">The department to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created department with updated information.</returns>
        Task<Department> CreateAsync(Department dept);

        /// <summary>
        /// Asynchronously updates an existing department.
        /// </summary>
        /// <param name="dept">The department with updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated department.</returns>

        /// <summary>
        /// Asynchronously updates an existing department.
        /// </summary>
        /// <param name="dept">The department with updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated department.</returns>
        Task<Department> UpdateAsync(Department dept);

        /// <summary>
        /// Asynchronously deletes a department.
        /// </summary>
        /// <param name="dept">The department to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted department.</returns>
        Task<Department> DeleteAsync(Department dept);
    }
}

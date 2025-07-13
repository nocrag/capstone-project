using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Retrieves all departments as DTOs.
        /// </summary>
        /// <returns>A list of <see cref="DepartmentDTO"/> objects.</returns>
        List<DepartmentDTO> GetAll();

        /// <summary>
        /// Retrieves a department entity by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>The corresponding <see cref="Department"/> entity.</returns>
        Department GetDept(int id);

        /// <summary>
        /// Converts a <see cref="DepartmentDTO"/> to a <see cref="Department"/> entity.
        /// </summary>
        /// <param name="deptDtO">The department DTO.</param>
        /// <returns>The corresponding <see cref="Department"/> entity.</returns>
        Department PopulateDepartmentFromDTO(DepartmentDTO deptDtO);

        /// <summary>
        /// Retrieves a department DTO by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>The corresponding <see cref="DepartmentDTO"/>.</returns>
        DepartmentDTO Get(int id);

        /// <summary>
        /// Creates a new department entity.
        /// </summary>
        /// <param name="department">The department entity to create.</param>
        /// <returns>The created <see cref="Department"/> entity.</returns>
        Department Create(Department department);

        /// <summary>
        /// Updates an existing department entity.
        /// </summary>
        /// <param name="department">The department entity with updated data.</param>
        /// <returns>The updated <see cref="Department"/> entity.</returns>
        Department Update(Department department);

        /// <summary>
        /// Deletes a department entity.
        /// </summary>
        /// <param name="department">The department entity to delete.</param>
        /// <returns>The deleted <see cref="Department"/> entity.</returns>
        Department Delete(Department department);

        /// <summary>
        /// Validates the given department entity.
        /// </summary>
        /// <param name="department">The department entity to validate.</param>
        void Validate(Department department);

        /// <summary>
        /// Checks if a department name is unique.
        /// </summary>
        /// <param name="name">The name to check for uniqueness.</param>
        /// <returns><c>true</c> if the name is unique; otherwise, <c>false</c>.</returns>
        bool IsDepartmentNameUnique(string name);

        /// <summary>
        /// Asynchronously retrieves all departments as DTOs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="DepartmentDTO"/> objects.</returns>
        Task<List<DepartmentDTO>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a department DTO by its ID.
        /// </summary>
        /// <param name="id">The department ID.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="DepartmentDTO"/>.</returns>
        Task<DepartmentDTO> GetAsync(int id);

        /// <summary>
        /// Asynchronously creates a new department entity.
        /// </summary>
        /// <param name="department">The department entity to create.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="Department"/> entity.</returns>
        Task<Department> CreateAsync(Department department);

        /// <summary>
        /// Asynchronously updates an existing department entity.
        /// </summary>
        /// <param name="department">The department entity with updated data.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Department"/> entity.</returns>
        Task<Department> UpdateAsync(Department department);

        /// <summary>
        /// Asynchronously deletes a department entity.
        /// </summary>
        /// <param name="department">The department entity to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deleted <see cref="Department"/> entity.</returns>
        Task<Department> DeleteAsync(Department department);

    }
}

using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IEmploymentStatusRepo
    {
        /// <summary>
        /// Retrieves an <see cref="EmploymentStatus"/> by its ID.
        /// </summary>
        /// <param name="statusId">The ID of the employment status.</param>
        /// <returns>The corresponding <see cref="EmploymentStatus"/>.</returns>
        EmploymentStatus Get(int statusId);

        /// <summary>
        /// Retrieves all employment statuses.
        /// </summary>
        /// <returns>A list of <see cref="EmploymentStatus"/> objects.</returns>
        List<EmploymentStatus> GetAll();

        /// <summary>
        /// Asynchronously retrieves an <see cref="EmploymentStatus"/> by its ID.
        /// </summary>
        /// <param name="statusId">The ID of the employment status.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="EmploymentStatus"/>.</returns>
        Task<EmploymentStatus> GetAsync(int statusId);

        /// <summary>
        /// Asynchronously retrieves all employment statuses.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmploymentStatus"/> objects.</returns>
        Task<List<EmploymentStatus>> GetAllAsync();

    }
}

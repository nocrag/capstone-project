using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IJobRepo
    {
        /// <summary>
        /// Retrieves a <see cref="Job"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>The corresponding <see cref="Job"/>.</returns>
        Job Get(int id);

        /// <summary>
        /// Retrieves all jobs.
        /// </summary>
        /// <returns>A list of <see cref="Job"/> objects.</returns>
        List<Job> GetAll();

        /// <summary>
        /// Asynchronously retrieves a <see cref="Job"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="Job"/>.</returns>
        Task<Job> GetAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all jobs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Job"/> objects.</returns>
        Task<List<Job>> GetAllAsync();


    }
}

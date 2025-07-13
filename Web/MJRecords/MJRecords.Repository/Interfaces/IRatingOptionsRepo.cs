using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IRatingOptionsRepo
    {
        /// <summary>
        /// Retrieves a <see cref="RatingOptions"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the rating option.</param>
        /// <returns>The corresponding <see cref="RatingOptions"/>.</returns>
        RatingOptions Get(int id);

        /// <summary>
        /// Retrieves all rating options.
        /// </summary>
        /// <returns>A list of <see cref="RatingOptions"/> objects.</returns>
        List<RatingOptions> GetAll();

        /// <summary>
        /// Asynchronously retrieves a <see cref="RatingOptions"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the rating option.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the corresponding <see cref="RatingOptions"/>.</returns>
        Task<RatingOptions> GetAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all rating options.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="RatingOptions"/> objects.</returns>
        Task<List<RatingOptions>> GetAllAsync();

    }
}

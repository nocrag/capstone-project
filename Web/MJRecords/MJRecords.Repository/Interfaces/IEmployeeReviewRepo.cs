using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface IEmployeeReviewRepo
    {
        /// <summary>
        /// Retrieves all employee reviews.
        /// </summary>
        /// <returns>A list of <see cref="EmployeeReviewDTO"/> objects.</returns>
        List<EmployeeReviewDTO> GetAll();

        /// <summary>
        /// Retrieves a specific employee review by its ID.
        /// </summary>
        /// <param name="id">The ID of the employee review.</param>
        /// <returns>An <see cref="EmployeeReviewDTO"/> representing the review.</returns>
        EmployeeReviewDTO Get(int id);

        /// <summary>
        /// Creates a new employee review for the specified quarter and year.
        /// </summary>
        /// <param name="empReview">The <see cref="EmployeeReview"/> entity to be created.</param>
        /// <param name="quarter">The quarter of the review (1–4).</param>
        /// <param name="year">The year of the review.</param>
        /// <returns>The created <see cref="EmployeeReview"/> entity.</returns>
        EmployeeReview Create(EmployeeReview empReview, int quarter, int year);

        /// <summary>
        /// Finds employees who do not have a review in the specified quarter and year.
        /// </summary>
        /// <param name="quarter">The quarter to check (1–4).</param>
        /// <param name="year">The year to check.</param>
        /// <returns>A list of <see cref="EmployeeReviewValidationResultDTO"/> representing missing reviews.</returns>
        List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarter(int quarter, int year);

        /// <summary>
        /// Finds employees under a specific supervisor who do not have a review in the specified quarter and year.
        /// </summary>
        /// <param name="id">The ID of the supervisor.</param>
        /// <param name="quarter">The quarter to check (1–4).</param>
        /// <param name="year">The year to check.</param>
        /// <returns>A list of <see cref="EmployeeReviewValidationResultDTO"/> representing missing reviews.</returns>
        List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarterBySupervisor(string id, int quarter, int year);

        /// <summary>
        /// Retrieves all reviews made by a specific supervisor.
        /// </summary>
        /// <param name="id">The ID of the supervisor.</param>
        /// <returns>A list of <see cref="EmployeeReviewListDTO"/> representing the supervisor's reviews.</returns>
        List<EmployeeReviewListDTO> GetAllReviewsMadeBySupervisor(string id);

        /// <summary>
        /// Asynchronously retrieves all employee reviews.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeReviewDTO"/> objects.</returns>
        Task<List<EmployeeReviewDTO>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a specific employee review by its ID.
        /// </summary>
        /// <param name="id">The ID of the employee review.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="EmployeeReviewDTO"/>.</returns>
        Task<EmployeeReviewDTO> GetAsync(int id);

        /// <summary>
        /// Asynchronously creates a new employee review for the specified quarter and year.
        /// </summary>
        /// <param name="empReview">The <see cref="EmployeeReview"/> entity to be created.</param>
        /// <param name="quarter">The quarter of the review (1–4).</param>
        /// <param name="year">The year of the review.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="EmployeeReview"/> entity.</returns>
        Task<EmployeeReview> CreateAsync(EmployeeReview empReview, int quarter, int year);

        /// <summary>
        /// Asynchronously finds employees who do not have a review in the specified quarter and year.
        /// </summary>
        /// <param name="quarter">The quarter to check (1–4).</param>
        /// <param name="year">The year to check.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeReviewValidationResultDTO"/> representing missing reviews.</returns>
        Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterAsync(int quarter, int year);

        /// <summary>
        /// Asynchronously finds employees under a specific supervisor who do not have a review in the specified quarter and year.
        /// </summary>
        /// <param name="id">The ID of the supervisor.</param>
        /// <param name="quarter">The quarter to check (1–4).</param>
        /// <param name="year">The year to check.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeReviewValidationResultDTO"/> representing missing reviews.</returns>
        Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterBySupervisorAsync(string id, int quarter, int year);

        /// <summary>
        /// Asynchronously retrieves all reviews made by a specific supervisor.
        /// </summary>
        /// <param name="id">The ID of the supervisor.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="EmployeeReviewListDTO"/> representing the supervisor's reviews.</returns>
        Task<List<EmployeeReviewListDTO>> GetAllReviewsMadeBySupervisorAsync(string id);


    }
}

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
    public class EmployeeReviewService : IEmployeeReviewService
    {
        private readonly IEmployeeReviewRepo _repo;

        public EmployeeReviewService(IEmployeeReviewRepo repo)
        {
            _repo = repo;
        }

        public EmployeeReview Create(EmployeeReview empReview, int quarter, int year)
        {
            Validate(empReview, quarter, year);
            if (empReview.Errors.Count == 0)
            {
                return _repo.Create(empReview, quarter, year);
            }
            
            return empReview;
        }

        public async Task<EmployeeReview> CreateAsync(EmployeeReview empReview, int quarter, int year)
        {
            Validate(empReview, quarter, year);
            if (empReview.Errors.Count == 0)
            {
                return await _repo.CreateAsync(empReview, quarter, year);
            }

            return empReview;
        }

        public List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarter(int quarter, int year)
        {
            return _repo.FindEmployeesWithoutReviewInQuarter(quarter, year);
        }

        public async Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterAsync(int quarter, int year)
        {
            return await _repo.FindEmployeesWithoutReviewInQuarterAsync(quarter, year);
        }

        public List<EmployeeReviewValidationResultDTO> FindEmployeesWithoutReviewInQuarterBySupervisor(string id, int quarter, int year)
        {
            return _repo.FindEmployeesWithoutReviewInQuarterBySupervisor(id, quarter, year);
        }

        public async Task<List<EmployeeReviewValidationResultDTO>> FindEmployeesWithoutReviewInQuarterBySupervisorAsync(string id, int quarter, int year)
        {
            return await _repo.FindEmployeesWithoutReviewInQuarterBySupervisorAsync(id, quarter, year);
        }

        public EmployeeReviewDTO Get(int id)
        {
            return _repo.Get(id);
        }

        public List<EmployeeReviewDTO> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<EmployeeReviewDTO>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public List<EmployeeReviewListDTO> GetAllReviewsMadeBySupervisor(string id)
        {
            return _repo.GetAllReviewsMadeBySupervisor(id);
        }

        public async Task<List<EmployeeReviewListDTO>> GetAllReviewsMadeBySupervisorAsync(string id)
        {
            return await _repo.GetAllReviewsMadeBySupervisorAsync(id);
        }

        public async Task<EmployeeReviewDTO> GetAsync(int id)
        {
            return await _repo.GetAsync(id);
        }

        public void Validate(EmployeeReview empReview, int quarter, int year)
        {
            // 1. Validate data annotations
            List<ValidationResult> results = new();
            Validator.TryValidateObject(empReview, new ValidationContext(empReview), results, true);

            // check here
            foreach (ValidationResult e in results)
            {
                empReview.AddError(new(e.ErrorMessage, ErrorType.Model));
            }

            List<EmployeeReviewValidationResultDTO> foundEmployeeForReview = FindEmployeesWithoutReviewInQuarter(quarter, year).Where(e => e.EmployeeId == empReview.EmployeeId).ToList();
            
            // if there are no results found for the specific quarter then that means the employee already have a review for it
            if (foundEmployeeForReview.Count == 0)
            {
                empReview.AddError(new("The Employee Already have a review for this quarter", ErrorType.Business));
            }

            if (empReview.ReviewDate.Date > DateTime.Now.Date)
            {
                empReview.AddError(new("The review date must not be set in the future", ErrorType.Business));
            }

        }

    }
}

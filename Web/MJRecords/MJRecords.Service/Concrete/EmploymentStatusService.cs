using MJRecords.Model;
using MJRecords.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class EmploymentStatusService : IEmploymentStatusService
    {
        private readonly IEmploymentStatusRepo _repo;

        public EmploymentStatusService(IEmploymentStatusRepo repo)
        {
            _repo = repo;
        }

        public EmploymentStatus Get(int statusId)
        {
            return _repo.Get(statusId);
        }

        public List<EmploymentStatus> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<EmploymentStatus>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<EmploymentStatus> GetAsync(int statusId)
        {
            return await _repo.GetAsync(statusId);
        }
    }
}

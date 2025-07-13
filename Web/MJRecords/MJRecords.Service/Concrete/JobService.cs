using MJRecords.Model;
using MJRecords.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepo _repo;

        public JobService(IJobRepo repo)
        {
            _repo = repo;
        }

        public Job Get(int id)
        {
            return _repo.Get(id);
        }

        public List<Job> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Job> GetAsync(int id)
        {
            return await _repo.GetAsync(id);
        }
    }
}

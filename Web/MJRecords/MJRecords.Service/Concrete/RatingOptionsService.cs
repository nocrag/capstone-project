using MJRecords.Model;
using MJRecords.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class RatingOptionsService : IRatingOptionsService
    {
        private readonly IRatingOptionsRepo _repo;

        public RatingOptionsService(IRatingOptionsRepo repo)
        {
            _repo = repo;
        }

        public RatingOptions Get(int id)
        {
            return _repo.Get(id);
        }

        public List<RatingOptions> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<RatingOptions>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<RatingOptions> GetAsync(int id)
        {
            return await _repo.GetAsync(id);
        }
    }
}

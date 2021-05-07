using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Core.Repository
{
    public interface IJobRepository : IRepository<Job>
    {
        Task Add(Job job);

        Task<Job> GetById(int id);

        Task<ListItemResponse<Job>> GetAll(JobFilterModel filterModel);

        Task<ListItemResponse<Job>> GetAllByDate();

        Task Update(int id, JobUpdateModel job);

        Task<Job> Delete(int id);
    }
}

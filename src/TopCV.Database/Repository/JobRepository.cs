using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Database.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;
        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public  async Task Add(Job job) => await _context.Jobs.AddAsync(job);
   
        public  async Task Commit() =>  await _context.SaveChangesAsync();
        

        public async Task<ListItemResponse<Job>> GetAll(JobFilterModel filterModel)
        {
            IQueryable<Job> Jobs = _context.Jobs.Include(j=>j.Company);

            if(!string.IsNullOrWhiteSpace(filterModel.KeyWord))
            {
                filterModel.KeyWord = filterModel.KeyWord.Trim();

                Jobs = Jobs
                    .Where(j => j.Name.ToLower().Contains(filterModel.KeyWord.ToLower()) || j.Company.Name.ToLower().Contains(filterModel.KeyWord.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(filterModel.Location))
            {
                Jobs = Jobs.Where(j => j.Location == filterModel.Location);
            }

            /*if (filterModel.DateIndication != null)
            {
                if (filterModel.DateIndication < 0)
                    Jobs = Jobs.OrderByDescending(j => j.DateCreated);
                else if (filterModel.DateIndication > 0)
                    Jobs = Jobs.OrderBy(j => j.DateCreated);
                else if (filterModel.Date != null)
                    Jobs = Jobs.Where(j => j.DateCreated.Date == filterModel.Date.Value.Date);
            }*/
            if(filterModel.OrderBy != null)
            {
                switch(filterModel.OrderBy)
                {
                    case 1:
                        Jobs = Jobs.OrderByDescending(j => j.DateCreated);
                        break;
                    case 2:
                        Jobs = Jobs.OrderBy(j => j.DateCreated);
                        break;
                    case 3:
                        Jobs = Jobs.OrderByDescending(j => j.Salary);
                        break;
                    case 4:
                        Jobs = Jobs.OrderBy(j => j.Salary);
                        break;
                }
            }

            if(filterModel.CompanyId != null)
            {
                Jobs = Jobs.Where(j => j.CompanyId == filterModel.CompanyId);
            }

            return new ListItemResponse<Job>
            {
                Items = Jobs.ToList(),
                TotalItems = await Jobs.CountAsync()
            };
        }
        public async Task<ListItemResponse<Job>> GetAllByDate()
        {
            var Jobs = _context.Jobs.Include(j => j.Company);

            //Jobs = Jobs.OrderByDescending(job => job.DateCreated);

            return new ListItemResponse<Job>
            {
                Items = Jobs.ToList(),
                TotalItems = await Jobs.CountAsync()
            };
        }
        public async Task<Job> GetById(int id) => (await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j=>j.Id == id));
      

        public async Task Update(int id, JobUpdateModel job)
        {
            Job currentJob = await _context.Jobs.FindAsync(id);
            if (currentJob != null)
            {
                // modify properties
                currentJob.Name = job.Name;
                currentJob.Description = job.Description;
                currentJob.Benefit = job.Benefit;
                currentJob.Requirement = job.Requirement;
                currentJob.Location = job.Location;
            }
        }

        public async Task<Job> Delete(int id)
        {
            Job job = await _context.Jobs.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }
            return job;
        }
    }
}

    
    
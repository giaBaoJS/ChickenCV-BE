using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.PostModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Database.Repository
{
	public class CandidateRepository : ICandidateRepository
	{
		private readonly ApplicationDbContext _context;
		public CandidateRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Add(Candidate candidate) => await _context.Candidates.AddAsync(candidate);

		public async Task Commit() => await _context.SaveChangesAsync();

		public async Task<Candidate> Delete(int id)
		{
			Candidate candidate = await _context.Candidates.FindAsync(id);
			if (candidate != null)
			{
				_context.Candidates.Remove(candidate);
			}
			return candidate;
		}

		public async Task<ListItemResponse<Candidate>> GetAll(CandidateFilterModel filterModel)
		{
			IQueryable<Candidate> candidates = _context.Candidates;

			if (filterModel.Id != null)
			{
				string id = filterModel.Id.ToString();
				candidates = candidates.Where(c => c.Id.ToString().Contains(id));
			}

			if (!string.IsNullOrEmpty(filterModel.Name))
				candidates = candidates.Where(c => c.Name.ToLower().Contains(filterModel.Name.ToLower()));

			int totalItems = await candidates.CountAsync();

			if (filterModel.Skip != null && filterModel.Skip > 0)
				candidates = candidates.Skip((int)filterModel.Skip);

			if (filterModel.Offset != null && filterModel.Offset > 0)
				candidates = candidates.Take((int)filterModel.Offset);

			return new ListItemResponse<Candidate>
			{
				Items = await candidates.ToListAsync(),
				TotalItems = totalItems
			};
		}

		public async Task<Candidate> GetById(int id) => await _context.Candidates.FindAsync(id);

		public async Task Update(int id, CandidateUpdateModel candidate)
		{
			Candidate currentCandidate = await _context.Candidates.FindAsync(id);
			if (currentCandidate != null)
			{
				// modify properties
				currentCandidate.Name = candidate.Name;
			}
		}

		public async Task ApplyForJob(ApplyForJobModel data){
			ApplicationDetail newApply = new ApplicationDetail();
			newApply.CVId = data.CVId;
			newApply.JobId = data.JobId;
			await _context.ApplicationDetails.AddAsync(newApply);
		}
		
		public async Task<ApplicationDetail> DeleteApply(int CVid, int Jobid)
		{
			ApplicationDetail apply = await _context.ApplicationDetails.FindAsync(CVid, Jobid);
			//await _context.ApplicationDetails.FindAsync(new { CVId=1, JobId=2 });
			if (apply != null)
			{
				_context.ApplicationDetails.Remove(apply);
			}
			return apply;
		}
	}
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.PostModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;

namespace TopCV.Database.Repository
{
	public class CVResponsitory : ICVResponsitory
	{
		private readonly ApplicationDbContext _context;
		public CVResponsitory(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task Commit() => await _context.SaveChangesAsync();

		public async Task Add(CVAddModel data)
		{
			var fileName = data.file.FileName;
			CV cv = new CV();
			cv.Name = data.Name;
			cv.CandidateId = data.CandidateId;
			cv.Path = "https://localhost:44361/uploads/"+fileName;
			await _context.CVs.AddAsync(cv);
		}
		public async Task<ListItemResponse<CV>> Get( int? id)
		{
			IQueryable<CV> cVs = _context.CVs;

			if (id != null)
			{
				cVs = cVs.Where(cv => cv.CandidateId == id);
				
			}
			int totalItems = await cVs.CountAsync();
			return new ListItemResponse<CV>
			{
				Items = await cVs.ToListAsync(),
				TotalItems = totalItems
			};
		}
		public async Task<CV> Delete(int id)
		{
			CV cv = await _context.CVs.FindAsync(id);
			if (cv != null)
			{
				_context.CVs.Remove(cv);
			}
			return cv;
		}

		public async Task<ListItemResponse<ApplicationDetail>> GetCandidateAppliced(string mode, int id)
		{
			IQueryable<ApplicationDetail> applicationDetails = _context.ApplicationDetails
				.Include(ad => ad.CV)
				.Include(ad =>ad.Job);

			if(mode == "candidate") applicationDetails = applicationDetails.Where(ad => ad.CV.CandidateId == id);
			else if(mode =="company") applicationDetails = applicationDetails.Where(ad => ad.Job.CompanyId == id);


			int totalItems = await applicationDetails.CountAsync();
			return new ListItemResponse<ApplicationDetail>
			{
				Items = await applicationDetails.ToListAsync(),
				TotalItems = totalItems
			};
		}


	}
}

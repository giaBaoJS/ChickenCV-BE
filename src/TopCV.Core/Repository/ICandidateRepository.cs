using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.PostModel;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Core.Repository
{
	public interface ICandidateRepository : IRepository<Candidate>
	{
		Task Add(Candidate candidate);

		Task<Candidate> GetById(int id);

		Task<ListItemResponse<Candidate>> GetAll(CandidateFilterModel filterModel);

		Task Update(int id, CandidateUpdateModel candidate);

		Task<Candidate> Delete(int id);
		Task ApplyForJob(ApplyForJobModel data);
		Task<ApplicationDetail> DeleteApply(int CVid, int Jobid);
	}
}

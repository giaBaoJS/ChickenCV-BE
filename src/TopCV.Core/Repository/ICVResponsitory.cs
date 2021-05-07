using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.PostModel;
using TopCV.Core.Shared;

namespace TopCV.Core.Repository
{
	public interface ICVResponsitory : IRepository<CV>
	{
		Task Add(CVAddModel data);
		Task<ListItemResponse<CV>> Get( int? id);
		Task<CV> Delete(int CandidateId);
		Task<ListItemResponse<ApplicationDetail>> GetCandidateAppliced(string mode, int id);
	}
}

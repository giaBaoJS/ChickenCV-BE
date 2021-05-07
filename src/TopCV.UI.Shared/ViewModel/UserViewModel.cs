using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.UI.Shared.ViewModel
{
	public class UserViewModel
	{
		public int Id{ get; set; }
		public string UserName{ get; set; }
		public CompanyViewModel Company{ get; set; }
		public CandidateViewModel Candidate{ get; set; }
		public IEnumerable<int> RoleIds{ get; set; }
	}
}

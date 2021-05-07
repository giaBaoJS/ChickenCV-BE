using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.UI.Shared.ViewModel
{
	public class CVViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateModified { get; set; }
		public string Path { get; set; }
		public int CandidateId { get; set; }
	}
}

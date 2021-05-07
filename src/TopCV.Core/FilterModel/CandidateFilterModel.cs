using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.FilterModel
{
	public class CandidateFilterModel
	{
		public int? Id { get; set; }
		public string Name { get; set; }

		public int? Skip{ get; set; }
		public int? Offset{ get; set; }
	}
}

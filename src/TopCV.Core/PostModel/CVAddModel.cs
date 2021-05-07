using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.PostModel
{
	public class CVAddModel
	{
		public string Name { get; set; }
		public int CandidateId { get; set; }
		public IFormFile file { get; set; }
	}
}

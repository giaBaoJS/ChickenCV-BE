using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.PostModel
{
	public class ApplyForJobModel
	{
		public int CVId { get; set; }
		public int JobId { get; set; }
		public DateTime DateCreated { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.UI.Shared.ViewModel
{
	public class ApplicationDetailViewModel
	{
		public int CVId { get; set; }
		public CVViewModel CV { get; set; }
		public int JobId { get; set; }
		public JobViewModel Job { get; set; }
		public DateTime DateCreated { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.UI.Shared.CreateModel
{
    public class JobCreateModel
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public string Requirement { get; set; }
		public string Benefit { get; set; }
		public decimal Salary { get; set; }
		public string Location { get; set; }
		public int CompanyId { get; set; }
	}
}

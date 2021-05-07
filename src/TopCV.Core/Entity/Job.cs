using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class Job
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string Requirement { get; set; }

		[Required]
		public string Benefit { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public decimal Salary { get; set; }

		[Required]
		public string Location { get; set; }

		[Required]
		public int CompanyId { get; set; }
		[ForeignKey(nameof(CompanyId))]
		public virtual Company Company { get; set; }

		[InverseProperty(nameof(ApplicationDetail.Job))]
		public virtual ICollection<ApplicationDetail> ApplicationDetails { get; } = new List<ApplicationDetail>();
	}
}

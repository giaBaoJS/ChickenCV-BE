using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class CV
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public DateTime DateModified { get; set; }

		[Required]
		public string Path { get; set; }

		[Required]
		public int CandidateId{get;set;}
		[ForeignKey(nameof(CandidateId))]
		public virtual Candidate Candidate{ get; set; }

		[InverseProperty(nameof(ApplicationDetail.CV))]
		public virtual ICollection<ApplicationDetail> ApplicationDetails { get; } = new List<ApplicationDetail>();
	}
}

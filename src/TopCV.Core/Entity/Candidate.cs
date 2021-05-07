using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class Candidate
	{
		[Key]
		public int Id{ get; set; }

		[Required]
		public string Name{ get; set; }

		[Required]
		public int UserId{ get; set; }
		[ForeignKey(nameof(UserId))]
		public virtual User User{ get; set; }

		[InverseProperty(nameof(CV.Candidate))]
		public virtual ICollection<CV> CVs { get; } = new List<CV>();
	}
}

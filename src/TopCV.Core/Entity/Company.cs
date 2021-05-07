using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class Company
	{
		[Key]
		public int Id{ get; set; }

		[Required]
		public string Name{ get; set; }

		[Required]
		public string PhoneNumber{ get; set; }

		[Required]
		public string Address{ get; set; }

		public int? UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }

		[InverseProperty(nameof(Job.Company))]
		public virtual ICollection<Job> Jobs { get; } = new List<Job>();
	}
}

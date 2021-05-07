using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class ApplicationDetail
	{
		[Required]
		public int CVId{ get; set; }
		[ForeignKey(nameof(CVId))]
		public virtual CV CV{ get; set; }

		[Required]
		public int JobId{ get; set; }
		[ForeignKey(nameof(JobId))]
		public virtual Job Job{ get; set; }

		public DateTime DateCreated{ get; set; }
	}
}

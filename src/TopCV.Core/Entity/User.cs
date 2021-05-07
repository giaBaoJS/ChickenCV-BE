using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class User : IdentityUser<int>
	{
		public bool IsDeleted { get; set; } = false;
		public DateTime? DateDeleted { get; set; }
		public virtual ICollection<IdentityUserRole<int>> UserRoles { get; } = new List<IdentityUserRole<int>>();

		public virtual Candidate Candidate{ get; set; }
		public virtual Company Company { get; set; }
	}
}

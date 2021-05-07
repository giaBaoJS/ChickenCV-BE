using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Entity
{
	public class Role : IdentityRole<int>
	{
		public Role() : base()
		{ }
		public Role(string roleName) : base(roleName)
		{ }

		public virtual ICollection<IdentityUserRole<int>> UserRoles { get; } = new List<IdentityUserRole<int>>();
	}
}

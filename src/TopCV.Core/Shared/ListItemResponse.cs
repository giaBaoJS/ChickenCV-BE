using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Shared
{
	public class ListItemResponse<T>
	{
		public IEnumerable<T> Items { get; set; }
		public int TotalItems { get; set; }
	}
}

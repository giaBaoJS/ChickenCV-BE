using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.Repository
{
	public interface IRepository<T>
	{
		Task Commit();
	}
}

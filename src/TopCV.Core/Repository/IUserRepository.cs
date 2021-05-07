using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Core.Repository
{
	public interface IUserRepository : IRepository<User>
	{
		Task Add(User user);

		Task<User> GetById(int id);

		Task<User> GetByUserName(string username);

		Task<ListItemResponse<User>> GetAll(UserFilterModel filterModel);

		Task Update(int id, UserUpdateModel user);

		Task<User> Delete(int id);
	}
}

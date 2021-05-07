using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;

namespace TopCV.Database.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Add(User user) => await _context.Users.AddAsync(user);

		public async Task Commit() => await _context.SaveChangesAsync();

		public async Task<User> Delete(int id)
		{
			User user = await _context.Users
				.Include(u => u.UserRoles)
				.Include(u => u.Company)
				.Include(u => u.Candidate)
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user != null)
			{
				user.IsDeleted = true;
				user.DateDeleted = DateTime.Now;
			}
			return user;
		}

		public async Task<ListItemResponse<User>> GetAll(UserFilterModel filterModel)
		{
			IQueryable<User> users = _context.Users
				.Include(u => u.UserRoles)
				.Include(u => u.Company)
				.Include(u => u.Candidate)
				.Where(u => u.IsDeleted == false);

			if(filterModel.Id != null)
			{
				string id = filterModel.Id.ToString();
				users = users.Where(u => u.Id.ToString().Contains(id));
			}

			if (!string.IsNullOrEmpty(filterModel.UserName))
				users = users.Where(u => u.UserName.ToLower().Contains(filterModel.UserName.ToLower()));

			if(filterModel.RoleId != null)
			{
				users = users.Where(u => u.UserRoles.Any(ur => ur.RoleId == filterModel.RoleId));
			}

			int totalItems = await users.CountAsync();

			if (filterModel.Skip != null && filterModel.Skip > 0)
				users = users.Skip((int)filterModel.Skip);

			if (filterModel.Offset != null && filterModel.Offset > 0)
				users = users.Take((int)filterModel.Offset);

			return new ListItemResponse<User>
			{
				Items = await users.ToListAsync(),
				TotalItems = totalItems
			};
		}

		public async Task<User> GetById(int id)
		{
			User user = await _context.Users
				.Include(u => u.UserRoles)
				.Include(u => u.Company)
				.Include(u => u.Candidate)
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user != null && user.IsDeleted == true)
				return null;
			return user;
		}

		public async Task<User> GetByUserName(string userName)
		{
			User user = await _context.Users
				.Include(u => u.UserRoles)
				.Include(u => u.Company)
				.Include(u => u.Candidate)
				.FirstOrDefaultAsync(u => u.UserName == userName);
			if (user != null && user.IsDeleted == true)
				return null;
			return user;
		}

		public async Task Update(int id, UserUpdateModel user)
		{
			User currentUser = await _context.Users.FindAsync(id);
			if(currentUser != null)
			{
				// modify properties
				//currentUser.UserName = user.UserName;
			}
		}
	}
}

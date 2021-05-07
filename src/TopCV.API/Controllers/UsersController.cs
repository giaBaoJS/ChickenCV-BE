using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;
using TopCV.UI.Shared.ViewModel;

namespace TopCV.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public UsersController(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper)
		{
			_userRepository = userRepository;
			_userManager = userManager;
			_mapper = mapper;
		}

		[HttpGet("{id:min(1):int}")]
		public async Task<ActionResult<UserViewModel>> GetById(int id)
		{
			if (id < 1)
				return NotFound(new { Message = "User not found" });

			User user = await _userRepository.GetById(id);

			if (user == null)
				return NotFound(new { Message = "User not found" });

			UserViewModel userModel = _mapper.Map<UserViewModel>(user);

			return Ok(userModel);
		}

		[HttpGet]
		public async Task<ActionResult<ListItemResponse<UserViewModel>>> GetAll(int? id, string userName, int? roleId, int? skip, int? offset)
		{
			UserFilterModel filterModel = new UserFilterModel
			{
				Id = id,
				UserName = userName,
				RoleId = roleId,
				Skip = skip,
				Offset = offset
			};

			ListItemResponse<User> usersList = await _userRepository.GetAll(filterModel);

			return Ok(new ListItemResponse<UserViewModel>
			{
				Items = usersList.Items.Select(u => _mapper.Map<UserViewModel>(u)),
				TotalItems = usersList.TotalItems
			});
		}

		[HttpPut("{id:min(1):int}")]
		public async Task<ActionResult> Update(int id, UserUpdateModel updateModel)
		{
			if (id < 1)
				return NotFound(new { Message = "User not found" });

			// validate updateModel properties here

			await _userRepository.Update(id, updateModel);

			await _userRepository.Commit();

			return Ok();
		}

		[HttpDelete("{id:min(1):int}")]
		public async Task<ActionResult<UserViewModel>> Delete(int id)
		{
			if (id < 1)
				return NotFound(new { Message = "User not found" });

			User user = await _userRepository.Delete(id);

			await _userRepository.Commit();

			return Ok(_mapper.Map<UserViewModel>(user));
		}
	}
}

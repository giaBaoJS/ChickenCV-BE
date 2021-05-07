using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.UI.Shared.Model;
using TopCV.UI.Shared.ViewModel;

namespace TopCV.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticateController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public AuthenticateController(UserManager<User> userManager, RoleManager<Role> roleManager, IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_userRepository = userRepository;
			_configuration = configuration;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			//var user = await _userManager.FindByNameAsync(model.Username);
			var user = await _userRepository.GetByUserName(model.Username);
			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var RoleType = await _userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name,user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
				};

				foreach (var userRole in RoleType)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

				var token = new JwtSecurityToken(
					issuer: _configuration["JWT:Issuer"],
					audience: _configuration["JWT:Audience"],
					expires: DateTime.Now.AddHours(3),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

				return Ok(new
				{
					success = true,
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo,
					user = _mapper.Map<UserViewModel>(user)
				});
			}
			return Unauthorized(new { Success = false, Message = "UserName or Password is not match" });
		}

		[HttpPost("register-candidate")]
		public async Task<IActionResult> RegisterCandidate([FromBody] CandidateRegisterModel model)
		{
			var userExists = await _userManager.FindByNameAsync(model.UserName);
			if (userExists != null){
				if (!userExists.IsDeleted)
					return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "User already exists!" });
				else return BadRequest(new { Success = false, Message = "UserName not allowed" });
			}
			
			if(string.IsNullOrWhiteSpace(model.UserName))
				return BadRequest(new { Success = false, Message = "UserName is required" });

			if (string.IsNullOrWhiteSpace(model.Password))
				return BadRequest(new { Success = false, Message = "Password is required" });

			if (string.IsNullOrWhiteSpace(model.Name))
				return BadRequest(new { Success = false, Message = "Name is required" });

			User user = new User()
			{
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
				Candidate = new Candidate
				{
					Name = model.Name
				}
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "User creation failed! Please check user details and try again." });

			if (!await _roleManager.RoleExistsAsync(RoleType.Admin))
				await _roleManager.CreateAsync(new Role(RoleType.Admin));
			if (!await _roleManager.RoleExistsAsync(RoleType.Candidate))
				await _roleManager.CreateAsync(new Role(RoleType.Candidate));
			if (!await _roleManager.RoleExistsAsync(RoleType.Company))
				await _roleManager.CreateAsync(new Role(RoleType.Company));


			await _userManager.AddToRoleAsync(user, RoleType.Candidate);

			return Ok(new { Success = true, Message = "User created successfully!" });
		}

		[HttpPost("register-company")]
		public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterModel model)
		{
			var userExists = await _userManager.FindByNameAsync(model.UserName);
			if (userExists != null)
			{
				if (!userExists.IsDeleted)
					return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "User already exists!" });
				else return BadRequest(new { Success = false, Message = "UserName not allowed" });
			}

			if (string.IsNullOrWhiteSpace(model.UserName))
				return BadRequest(new { Success = false, Message = "UserName is required" });

			if (string.IsNullOrWhiteSpace(model.Password))
				return BadRequest(new { Success = false, Message = "Password is required" });

			if (string.IsNullOrWhiteSpace(model.Name))
				return BadRequest(new { Success = false, Message = "Name is required" });

			if (string.IsNullOrWhiteSpace(model.PhoneNumber))
				return BadRequest(new { Success = false, Message = "Phone number is required" });

			/*if (new PhoneAttribute().IsValid(model.PhoneNumber))
				return BadRequest(new { Success = false, Message = "Phone number invalid" }); */

			if (string.IsNullOrWhiteSpace(model.Address))
				return BadRequest(new { Success = false, Message = "Address is required" });

			User user = new User()
			{
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
				Company = new Company
				{
					Name = model.Name,
					PhoneNumber = model.PhoneNumber,
					Address = model.Address
				}
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "User creation failed! Please check user details and try again." });

			if (!await _roleManager.RoleExistsAsync(RoleType.Admin))
				await _roleManager.CreateAsync(new Role(RoleType.Admin));
			if (!await _roleManager.RoleExistsAsync(RoleType.Candidate))
				await _roleManager.CreateAsync(new Role(RoleType.Candidate));
			if (!await _roleManager.RoleExistsAsync(RoleType.Company))
				await _roleManager.CreateAsync(new Role(RoleType.Company));

			await _userManager.AddToRoleAsync(user, RoleType.Company);

			return Ok(new { Success = true, Message = "User created successfully!" });
		}
	}
}

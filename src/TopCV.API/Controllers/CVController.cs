using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.PostModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.UI.Shared.ViewModel;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TopCV.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CVController : ControllerBase
	{
		private readonly ICVResponsitory _cVResponsitory;
		private readonly IMapper _mapper;
		private readonly IHostingEnvironment _environment;

		public CVController(ICVResponsitory cVResponsitory,  IMapper mapper, IHostingEnvironment environment)
		{
			_cVResponsitory = cVResponsitory;
			_mapper = mapper;
			_environment = environment;
		}
		[HttpGet]
		public async Task<IActionResult> GetCVs()
		{
			ListItemResponse<CV> cVs = await _cVResponsitory.Get(null);

			return Ok(new ListItemResponse<CVViewModel>
			{
				Items = cVs.Items.Select(u => _mapper.Map<CVViewModel>(u)),
				TotalItems = cVs.TotalItems
			});
		}

		[HttpGet("{id:min(1):int}")]
		public async Task<IActionResult> GetCVsByCandidate( int id)
		{
			ListItemResponse<CV> cVs = await _cVResponsitory.Get( id);

			return Ok(new ListItemResponse<CVViewModel>
			{
				Items = cVs.Items.Select(cv => _mapper.Map<CVViewModel>(cv)),
				TotalItems = cVs.TotalItems
			});
		}
		[HttpPost]
		public async Task<IActionResult> AddNewCV([FromForm] CVAddModel cv)
		{
			
			var uploads = Path.Combine(_environment.WebRootPath, "uploads");
			if (cv.file.Length > 0)
			{
				using (var fileStream = new FileStream(Path.Combine(uploads, cv.file.FileName), FileMode.Create))
				{
					await cv.file.CopyToAsync(fileStream);
				}
			}
			
			await _cVResponsitory.Add(cv);
			await _cVResponsitory.Commit();
			return Ok();
		}
		[HttpDelete("{id:min(1):int}")]
		public async Task<IActionResult> GetById(int id)
		{
			await _cVResponsitory.Delete(id);
			await _cVResponsitory.Commit();

			return Ok();
		}

		[HttpGet("{mode}/{id:min(1):int}")]
		public async Task<ActionResult<ListItemResponse<ApplicationDetailViewModel>>> GetCandidateAppliced(string mode,int id)
		{
			ListItemResponse<ApplicationDetail> candidatesList = await _cVResponsitory.GetCandidateAppliced(mode, id);

			return Ok(new ListItemResponse<ApplicationDetailViewModel>
			{
				Items = candidatesList.Items.Select(u => _mapper.Map<ApplicationDetailViewModel>(u)),
				TotalItems = candidatesList.TotalItems
			});
		}
	}
}

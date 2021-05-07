using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.Core.FilterModel;
using TopCV.Core.PostModel;
using TopCV.Core.Repository;
using TopCV.Core.Shared;
using TopCV.Core.UpdateModel;
using TopCV.Database.Repository;
using TopCV.UI.Shared.ViewModel;

namespace TopCV.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CandidatesController : ControllerBase
	{
		private readonly ICandidateRepository _candidateRepository;
		private readonly IMapper _mapper;

		public CandidatesController(ICandidateRepository candidateRepository, IMapper mapper)
		{
			_candidateRepository = candidateRepository;
			_mapper = mapper;
		}

		[HttpGet("{id:min(1):int}")]
		public async Task<ActionResult<CandidateViewModel>> GetById(int id)
		{
			if (id < 1)
				return NotFound(new { Message = "Candidate not found" });

			Candidate candidate = await _candidateRepository.GetById(id);

			if (candidate == null)
				return NotFound(new { Message = "Candidate not found" });

			CandidateViewModel candidateModel = _mapper.Map<CandidateViewModel>(candidate);

			return Ok(candidateModel);
		}

		[HttpGet]
		public async Task<ActionResult<ListItemResponse<CandidateViewModel>>> GetAll(int? id, string name, int? skip, int? offset)
		{
			CandidateFilterModel filterModel = new CandidateFilterModel
			{
				Id = id,
				Name = name,
				Skip = skip,
				Offset = offset
			};

			ListItemResponse<Candidate> candidatesList = await _candidateRepository.GetAll(filterModel);

			return Ok(new ListItemResponse<CandidateViewModel>
			{
				Items = candidatesList.Items.Select(u => _mapper.Map<CandidateViewModel>(u)),
				TotalItems = candidatesList.TotalItems
			});
		}

		[HttpPut("{id:min(1):int}")]
		public async Task<ActionResult> Update(int id, CandidateUpdateModel updateModel)
		{
			if (id < 1)
				return NotFound(new { Message = "Candidate not found" });

			// validate updateModel properties here
			if (string.IsNullOrWhiteSpace(updateModel.Name))
				return BadRequest(new { Message = "Name is invalid" });

			await _candidateRepository.Update(id, updateModel);

			await _candidateRepository.Commit();

			return Ok();
		}

		/*[HttpDelete("{id:min(1):int}")]
		public async Task<ActionResult<CandidateViewModel>> Delete(int id)
		{
			if (id < 1)
				return NotFound(new { Message = "Candidate not found" });

			Candidate candidate = await _candidateRepository.Delete(id);

			await _candidateRepository.Commit();

			return Ok(_mapper.Map<CandidateViewModel>(candidate));
		}*/

		[HttpPost("applyforjob")]
		public async Task<IActionResult> ApplyForJob(ApplyForJobModel data)
		{
			await _candidateRepository.ApplyForJob(data);
			await _candidateRepository.Commit();

			return Ok();
		}

		[HttpDelete("applyforjob/{CVid:min(1):int}/{Jobid:min(1):int}")]
		public async Task<IActionResult> DeleteApplyForJob(int CVid,int Jobid)
		{
			await _candidateRepository.DeleteApply(CVid,Jobid);
			await _candidateRepository.Commit();

			return Ok();
		}

		
	}
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using TopCV.UI.Shared.CreateModel;
using TopCV.UI.Shared.ViewModel;

namespace TopCV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public JobsController(IJobRepository jobsRepository, IMapper mapper)
        {
            _jobRepository = jobsRepository;
            _mapper = mapper;
        }

        [HttpPost]

        public async Task<ActionResult> CreateJob(JobCreateModel jobCreateModel)
        {
            if(string.IsNullOrWhiteSpace(jobCreateModel.Name))
                return BadRequest(new { Success = false, Message = "JobName is required" });
            if (string.IsNullOrWhiteSpace(jobCreateModel.Location))
                return BadRequest(new { Success = false, Message = "Location is required" });

            if (jobCreateModel.Salary<1)
                return BadRequest(new { Success = false, Message = "Salary must be greater than 0" });

            if (string.IsNullOrWhiteSpace(jobCreateModel.Description))
                return BadRequest(new { Success = false, Message = "Description is required" });
            if (string.IsNullOrWhiteSpace(jobCreateModel.Requirement))

                return BadRequest(new { Success = false, Message = "Message is  required" });
            if (string.IsNullOrWhiteSpace(jobCreateModel.Benefit))

                return BadRequest(new { Success = false, Message = "Benifit is required" });

            // check CompanyId here

            await _jobRepository.Add(_mapper.Map<Job>(jobCreateModel));
            await _jobRepository.Commit();
            return Ok(new { Success = true, Message = "Success" });
        }

        [HttpGet("{id:min(1):int}")]
        public async Task<ActionResult<JobViewModel>> GetById(int id)
        {
            if (id < 1)
                return NotFound(new { Message = "Job not found" });

            Job job = await _jobRepository.GetById(id);

            if (job == null)
                return NotFound(new { Message = "Job not found" });

            JobViewModel jobModel = _mapper.Map<JobViewModel>(job);

            return Ok(jobModel);
        }

        [HttpGet]
        public async Task<ActionResult<ListItemResponse<JobViewModel>>> GetAll(string keyWord, int? id, string? name, int? companyId, short? orderBy, string location, /*DateTime? date, short? dateIndication,*/ int? skip, int? offset)
        {
            JobFilterModel filterModel = new JobFilterModel
            {
                KeyWord = keyWord,
                Id = id,
                Name = name,
                /*Date = date,
                DateIndication = dateIndication,*/
                Location = location,
                OrderBy = orderBy,
                CompanyId = companyId,
                Skip = skip,
                Offset = offset
            };

            ListItemResponse<Job> jobsList = await _jobRepository.GetAll(filterModel);

            return Ok(new ListItemResponse<JobViewModel>
            {
                Items = jobsList.Items.Select(u => _mapper.Map<JobViewModel>(u)),
                TotalItems = jobsList.TotalItems
            });
        }

        [HttpPut("{id:min(1):int}")]
        public async Task<ActionResult> Update(int id, JobUpdateModel updateModel)
        {
            if (id < 1)
                return NotFound(new { Message = "Job not found" });

            // validate updateModel properties here
            if (string.IsNullOrWhiteSpace(updateModel.Name))
                return BadRequest(new { Message = "Name is invalid" });
            if (string.IsNullOrWhiteSpace(updateModel.Description))
                return BadRequest(new { Message = "Description is invalid" });
            if (string.IsNullOrWhiteSpace(updateModel.Benefit))
                return BadRequest(new { Message = "Benefit is invalid" });
            if (string.IsNullOrWhiteSpace(updateModel.Requirement))
                return BadRequest(new { Message = "Requirement is invalid" });
            if (string.IsNullOrWhiteSpace(updateModel.Location))
                return BadRequest(new { Message = "Location is invalid" });

            await _jobRepository.Update(id, updateModel);

            await _jobRepository.Commit();

            return Ok();
        }

        [HttpDelete("{id:min(1):int}")]
        public async Task<ActionResult<JobViewModel>> Delete(int id)
        {
        if (id < 1)
            return NotFound(new { Message = "Job not found" });

        Job job = await _jobRepository.Delete(id);

        await _jobRepository.Commit();

        return Ok(_mapper.Map<JobViewModel>(job));
        }
    }

}

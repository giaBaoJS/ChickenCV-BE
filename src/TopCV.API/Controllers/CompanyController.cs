using AutoMapper;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TopCV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        // GET: api/<CompanyController>
        [HttpGet]
        public async Task<ActionResult<ListItemResponse<CompanyViewModel>>> GetAll(int? id, string name)
        {
            CompanyFilterModel filterModel = new CompanyFilterModel
            {
                Id = id,
                Name = name,
            };

            ListItemResponse<Company> companyList = await _companyRepository.GetAll(filterModel);

            return Ok(new ListItemResponse<CompanyViewModel>
            {
                Items = companyList.Items.Select(u => _mapper.Map<CompanyViewModel>(u)),
                TotalItems = companyList.TotalItems
            });
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyViewModel>> GetById(int id)
        {
            if (id < 1)
                return NotFound(new { Message = "Company not found" });
            Company company = await _companyRepository.GetById(id);
            if (company == null)
                return NotFound(new { Message = "Candidate not found" });
            CompanyViewModel companyModel = _mapper.Map<CompanyViewModel>(company);

            return Ok(companyModel);
        }

        // POST api/<CompanyController>
        /*[HttpPost]
         public void Post([FromBody] string value)
        {
        }*/

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CompanyUpdateModel updateModel)
        {
            if (id < 1)
                return NotFound(new { Message = "Company not found" });

            // validate updateModel properties here
            if (string.IsNullOrWhiteSpace(updateModel.Name))
                return BadRequest(new { Message = "Name is invalid" });

            await _companyRepository.Update(id, updateModel);

            await _companyRepository.Commit();

            return Ok();
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

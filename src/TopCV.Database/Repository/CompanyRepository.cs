using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /* public Task Add(Company candidate) 
        {
            throw new NotImplementedException();
        }*/

        public async Task Commit() => await _context.SaveChangesAsync();

        public Task<Company> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Company> GetById(int id) => await _context.Companies.FindAsync(id);
        public async Task<ListItemResponse<Company>> GetAll(CompanyFilterModel filterModel)
        {
            IQueryable<Company> companies = _context.Companies;

            if (filterModel.Id != null)
            {
                string id = filterModel.Id.ToString();
                companies = companies.Where(c => c.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrEmpty(filterModel.Name))
                companies = companies.Where(c => c.Name.ToLower().Contains(filterModel.Name.ToLower()));

            int totalItems = await companies.CountAsync();

            return new ListItemResponse<Company>
            {
                Items = await companies.ToListAsync(),
                TotalItems = totalItems
            };
        }
        public async Task Update(int id, CompanyUpdateModel company)
        {
            Company currentCompany = await _context.Companies.FindAsync(id);
            if (currentCompany != null)
            {
                // modify properties
                currentCompany.Name = company.Name;
                currentCompany.PhoneNumber = company.PhoneNumber;
                currentCompany.Address = company.Address;
            }
        }
    }
}

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
    public interface ICompanyRepository : IRepository<Company>
    {        
        Task<Company> GetById(int id);

        Task<ListItemResponse<Company>> GetAll(CompanyFilterModel filterModel);

        Task Update(int id, CompanyUpdateModel candidate);
    }
}

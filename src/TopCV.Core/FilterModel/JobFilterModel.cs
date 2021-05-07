using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.Core.FilterModel
{
    public class JobFilterModel
    {
        public string KeyWord { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Benefit { get; set; }

        public string Requirement { get; set; }

        /*public DateTime? Date { get; set; }
        public short? DateIndication { get; set; }

        public decimal? Salary { get; set; }
        public short? SalaryIndication { get; set; }*/

        public short? OrderBy { get; set; }
        public string Location { get; set; }

        public int? CompanyId { get; set; }
        public int? Skip { get; set; }
        public int? Offset { get; set; }
    }
}

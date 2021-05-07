using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;
using TopCV.UI.Shared.CreateModel;

namespace TopCV.UI.Shared.ViewModel
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserViewModel>()
				.ForMember(vm => vm.RoleIds, options => options.MapFrom(u => u.UserRoles.Select(ur =>ur.RoleId)));

			CreateMap<Role, RoleViewModel>();

			CreateMap<Candidate, CandidateViewModel>();

			CreateMap<Job, JobViewModel>()
				.ForMember(vm =>vm.CompanyName,options=>options.MapFrom(j => j.Company !=null?j.Company.Name:null));

			CreateMap<JobCreateModel, Job>();
			CreateMap<CV, CVViewModel>();
			CreateMap<Company, CompanyViewModel>();
			CreateMap<ApplicationDetail, ApplicationDetailViewModel>();
		}
	}
}

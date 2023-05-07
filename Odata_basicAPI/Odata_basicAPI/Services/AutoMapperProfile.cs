using AutoMapper;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Model;

namespace Odata_basicAPI.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Define your mappings here
            CreateMap<Jobs, JobsDTO>();
            CreateMap<JobsDTO, Jobs>();
            CreateMap<Projects, ProjectsDTO>();
            CreateMap<ProjectsDTO, Projects>();
            CreateMap<Skills, SkillsDTO>();
            CreateMap<SkillsDTO, Skills>();
        }
    }
    
}

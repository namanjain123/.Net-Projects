using Odata_basicAPI.DTOs;
using Odata_basicAPI.Model;
namespace ProjectApi.Mapper
{
    public static class ProjectMapper
    {
        public static Projects ToModel(this ProjectsDTO dto)
        {
            return new Projects
            {
                Id = dto.Id,
                Project_Name = dto.Project_Name,
                Skills = dto.Skills,
                Description = dto.Description
            };
        }

        public static ProjectsDTO ToDTO(this Projects model)
        {
            return new ProjectsDTO
            {
                Id = model.Id,
                Description = model.Description,
                Skills = model.Skills,
                Project_Name = model.Project_Name
            };
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Odata_basicAPI.Model;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Services;
using System.Linq;
namespace ProjectApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase

    {
        private readonly IMapper _mapper;
        private readonly IMongoClient _dbClient;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public ProjectController(IMongoClient mongoClient, IMapper mapper)
        {
            _dbClient = mongoClient;
            _mapper = mapper;
        }
        [HttpGet]
        [ResponseCache(Duration = 40)] // Add this attribute to enable caching
        public async Task<JsonResult> Projects()
        {
            try
            {
                // Check if the result is already cached
                var cacheKey = "ProjectsList";
                var cachedResult = _cache.Get(cacheKey) as List<ProjectsDTO>;
                if (cachedResult != null)
                {
                    return new JsonResult(cachedResult);
                }

                // If the result is not cached, retrieve it from the database
                var dbList = await Task.Run(() => _dbClient.
                GetDatabase("Portfolio").
                GetCollection<Projects>("Projects").
                AsQueryable());
                if (dbList == null)
                {
                    return new JsonResult("There is some error");
                }
                
                var projectDTO = _mapper.Map<List<ProjectsDTO>>(dbList);

                // Add the result to the cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(cacheKey, projectDTO, cacheOptions);

                return new JsonResult(projectDTO);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ResponseCache(Duration = 3600)] // Add this attribute to enable caching
        public async Task<JsonResult> Projects(int id)
        {
            try
            {
                // Check if the result is already cached
                var cacheKey = "SkillsList";
                var cachedResult = _cache.Get(cacheKey) as List<ProjectsDTO>;
                if (cachedResult != null)
                {
                    return new JsonResult(cachedResult);
                }
                var filter = Builders<Projects>.Filter.Eq("Id", id);
                var dbList = await _dbClient
                    .GetDatabase("Portfolio")
                    .GetCollection<Projects>("Projects")
                    .FindAsync<Projects>(filter);
                
                var projectDTO = _mapper.Map<ProjectsDTO>(dbList);
                // Add the result to the cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, projectDTO, cacheOptions);
                return new JsonResult(projectDTO);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SkillAdd(ProjectsDTO project)
        {
            try
            {
                int gotDepartmentId = await _dbClient
                    .GetDatabase("Portfolio")
                    .GetCollection<Projects>("Projects")
                    .AsQueryable()
                    .CountAsync();
                project.Id = gotDepartmentId + 1;
                var projectDTO = _mapper.Map<Projects>(project);
                await _dbClient.GetDatabase("Portfolio").GetCollection<Projects>("Projects").InsertOneAsync(projectDTO);
                return new JsonResult("adde data", projectDTO);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPut]
        public async Task<JsonResult> SkillUpdate(ProjectsDTO project)
        {
            try
            {
                var config = new MapperConfiguration(map => {
                    map.CreateMap<Projects, ProjectsDTO>();
                });

                var mapper = config.CreateMapper();
                var projectDTO = mapper.Map<Projects>(project);
                var filter = Builders<Projects>.Filter.Eq("id", projectDTO.Id);
                var updateddata = Builders<Projects>
                    .Update
                        .Set("Project_Name", projectDTO.Project_Name)
                        .Set("Description", projectDTO.Description)
                        .Set("Skills", projectDTO.Skills);

                await _dbClient.GetDatabase("Portfolio").GetCollection<Projects>("Projects").UpdateOneAsync(filter, updateddata);
                return new JsonResult($"result is update for {projectDTO.Id}");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}


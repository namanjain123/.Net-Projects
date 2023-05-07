using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Odata_basicAPI.Services;
using Odata_basicAPI.Model;
using Odata_basicAPI.DTOs;

namespace ProjectApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IMongoClient _dbClient;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IMapper _mapper;
        public JobController(IMongoClient mongoClient, IMapper mapper)
        {
            _mapper = mapper;
            _dbClient = mongoClient;
        }
        [HttpPost]
        public async Task<JsonResult> JobsAdd(JobsDTO jobDTO)
        {
            try
            {
                int gotDepartmentId = await _dbClient
                    .GetDatabase("Portfolio")
                    .GetCollection<JobsDTO>("Jobs")
                    .AsQueryable()
                    .CountAsync();
                jobDTO.Id = gotDepartmentId + 1;
                var job = _mapper.Map<Jobs>(jobDTO);
                await _dbClient.GetDatabase("Portfolio").GetCollection<Jobs>("Jobs").InsertOneAsync(job);
                return new JsonResult("adde a new job");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet]
        [ResponseCache(Duration = 3600)] // Add this attribute to enable caching
        public async Task<JsonResult> Jobs()
        {
            try
            {
                var dbList = await Task.Run(() =>
                             _dbClient
                             .GetDatabase("Portfolio")
                             .GetCollection<Jobs>("Jobs")
                             .AsQueryable());
                
                var jobDTO = _mapper.Map<List<JobsDTO>>(dbList);

                return new JsonResult(jobDTO);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Model;

namespace Odata_basicAPI.OData
{

    [Route("api/odata/projects")]
    [ApiController]
    public class ProjectOdataController : ODataController
    {
        private readonly IMongoClient _dbClient;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IMapper _mapper;
        public ProjectOdataController(IMongoClient mongoClient, IMapper mapper)
        {
            _mapper = mapper;
            _dbClient = mongoClient;
        }
        [EnableQuery]
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

    }
}

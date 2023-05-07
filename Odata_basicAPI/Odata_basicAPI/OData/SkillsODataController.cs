using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Model;

namespace Odata_basicAPI.OData
{
    [ApiController]
    
    public class SkillsODataController : ODataController
    {
        private readonly IMongoClient _dbClient;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IMapper _mapper;
        public SkillsODataController(IMongoClient mongoClient, IMapper mapper)
        {
            _mapper = mapper;
            _dbClient = mongoClient;
        }
        
        
        [ODataRoute("Skills")]
        [EnableQuery]
        [ResponseCache(Duration = 40)] // Add this attribute to enable caching
        public async Task<IQueryable<SkillsDTO>> Skills(ODataQueryOptions<SkillsDTO> options)
        {
            try
            {
                // Check if the result is already cached
                var cacheKey = "SkillsList";
                var cachedResult = _cache.Get(cacheKey) as List<SkillsDTO>;
                if (cachedResult != null)
                {
                    return options.ApplyTo(cachedResult.AsQueryable()) as IQueryable<SkillsDTO>;
                }

                // If the result is not cached, retrieve it from the database
                var dbList = _dbClient.GetDatabase("Portfolio")
                                      .GetCollection<Skills>("Skills")
                                      .AsQueryable();
                if (dbList == null)
                {
                    throw new InvalidOperationException("There is some error");
                }
                var skillDTO = _mapper.Map<List<SkillsDTO>>(dbList);

                // Add the result to the cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(cacheKey, skillDTO, cacheOptions);

                return options.ApplyTo(skillDTO.AsQueryable()) as IQueryable<SkillsDTO>;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("There is some error");
            }
        }
        
    }
}

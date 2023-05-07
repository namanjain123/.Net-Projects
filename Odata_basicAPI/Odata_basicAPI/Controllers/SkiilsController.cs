using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Odata_basicAPI.Model;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Services;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace ProjectApi.Controllers
{
    
    
    [ApiController]
    [ODataAttributeRouting]
    public class SkiilsController : ODataController
    {
        private readonly IMongoClient _dbClient;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IMapper _mapper;
        public SkiilsController(IMongoClient mongoClient, IMapper mapper)
        {
            _mapper = mapper;
            _dbClient = mongoClient;
        }
        [EnableQuery]
        [ODataRoute("Skills")]
        [ResponseCache(Duration = 40)]
        public async Task<JsonResult> Skills(ODataQueryOptions<SkillsDTO> options)
        {
            try
            {
                // Check if the result is already cached
                var cacheKey = "SkillsList";
                var cachedResult = _cache.Get(cacheKey) as List<SkillsDTO>;
                if (cachedResult != null)
                {
                    return new JsonResult(cachedResult);
                }

                // If the result is not cached, retrieve it from the database
                var dbList = await Task.Run(() => _dbClient.
                GetDatabase("Portfolio").
                GetCollection<Skills>("Skills").
                AsQueryable());
                if (dbList == null)
                {
                    return new JsonResult("There is some error");
                }
                var skillDTO = _mapper.Map<List<SkillsDTO>>(dbList);

                // Add the result to the cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(cacheKey, skillDTO, cacheOptions);

                return new JsonResult(skillDTO);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ResponseCache(Duration = 3600)]
        public async Task<JsonResult> Skills(int id)
        {
            try
            {
                var cacheKey = $"Skill-{id}";
                var cachedResult = _cache.Get(cacheKey) as SkillsDTO;
                if (cachedResult != null)
                {
                    return new JsonResult(cachedResult);
                }
                var filter = Builders<Skills>.Filter.Eq("Id", id);
                var dbList = await _dbClient.GetDatabase("Portfolio").GetCollection<Skills>("Skills").FindAsync<Skills>(filter);
                var skillDTO = _mapper.Map<SkillsDTO>(dbList);
                // Add the result to the cache
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, skillDTO, cacheOptions);

                return new JsonResult(skillDTO); 
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SkillAdd(SkillsDTO skills)
        {
            try
            {
                int gotDepartmentId = await _dbClient.GetDatabase("Portfolio").GetCollection<Skills>("Skills").AsQueryable().CountAsync();
                if(gotDepartmentId < 0) {
                    return new JsonResult("Please Enter Some Text");
                }
                else 
                {
                    var skillDTO = _mapper.Map<Skills>(skills);
                    skillDTO.Id = gotDepartmentId + 1;
                await _dbClient.GetDatabase("Portfolio").GetCollection<Skills>("Skills").InsertOneAsync(skillDTO);
                return new JsonResult("added data");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPut]
        public async Task<JsonResult> SkillUpdate(SkillsDTO skills)
        {
            try
            {
                if (skills.Id == 0)
                {
                    return new JsonResult("You have missed the data");
                }
                else { 
                var filter = Builders<Skills>.Filter.Eq("Id", skills.Id);
                var updateddata = Builders<Skills>.Update
                        .Set("Skill", skills.Skill)
                        .Set("Rating", skills.Rating)
                        .Set("SubSet", skills.SubSet);
                await _dbClient.GetDatabase("Portfolio").GetCollection<Skills>("Skills").UpdateOneAsync(filter, updateddata);

                return new JsonResult($"result is update for {skills.Id}");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}


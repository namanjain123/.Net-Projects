using Api_For_Cosmo_DB.DTOs;
using Api_For_Cosmo_DB.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Api_For_Cosmo_DB.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NameController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly string CosmosDBAccountUri;
        private readonly string CosmosDBAccountPrimaryKey;
        private readonly string CosmosDbName;
        private readonly string CosmosDbContainerName;
        public NameController(IConfiguration config, IMapper mapper) {
            _config = config;
            CosmosDBAccountUri = _config.GetSection("Cosmo").GetSection("uri").Value ?? "";
            CosmosDBAccountPrimaryKey = _config.GetSection("Cosmo").GetSection("primarykey").Value??"";
            CosmosDbName = _config.GetSection("Cosmo").GetSection("dbName").Value ?? "";
            CosmosDbContainerName= _config.GetSection("Cosmo").GetSection("containername").Value ?? "";
            _mapper = mapper;
        }
        private Container ContainerClient()
        {
            CosmosClient cosmosDbClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
            Container containerClient = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return containerClient;
        }
        [HttpPost]
        public async Task<IActionResult> AddName(NameDTO name)
        {
            try
            {
                var container = ContainerClient();
                var data = _mapper.Map<NameModel>(name);
                var response = await container.CreateItemAsync(data, new PartitionKey(data.name));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNameDetailsById(string nameId, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<NameModel> response = await container.ReadItemAsync<NameModel>(nameId, new PartitionKey(partitionKey));
                var data = _mapper.Map<NameDTO>(response.Resource);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateName(NameDTO emp, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<NameModel> res = await container.ReadItemAsync<NameModel>(emp.id, new PartitionKey(partitionKey));
                //Get Existing Item
                var existingItem = res.Resource;
                //Replace existing item values with new values
                existingItem.name = emp.name;
                existingItem.LastName = emp.LastName;
                existingItem.FirstName = emp.FirstName;
                existingItem.Email = emp.Email;
                existingItem.Address = emp.Address;
                var updateRes = await container.ReplaceItemAsync(existingItem, emp.id, new PartitionKey(partitionKey));
                var nameOutputDto = _mapper.Map<NameDTO>(updateRes.Resource);
                return Ok(nameOutputDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteName(string empId, string partitionKey)
        {
            try
            {
                var container = ContainerClient();
                var response = await container.DeleteItemAsync<NameModel>(empId, new PartitionKey(partitionKey));
                return Ok(response.StatusCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

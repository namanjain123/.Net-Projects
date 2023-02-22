using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ChatGPT_WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ChatController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<string> solution(string text)
        {
            //Making Client
            HttpClient clinet = new HttpClient();
            //Token call
            string token = "Bearer " + _config["keys"];
            //Clenet defined
            clinet.DefaultRequestHeaders.Add("authorization", token);
            //call the inputs
            var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + text + "\",\"temperature\": 1,\"max_tokens\": 100}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await clinet.PostAsync("https://api.openai.com/v1/completions", content);
            string responseString = await response.Content.ReadAsStringAsync();
            try
            {
                var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

                string guess = dyData!.choices[0].text;
                return guess;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}

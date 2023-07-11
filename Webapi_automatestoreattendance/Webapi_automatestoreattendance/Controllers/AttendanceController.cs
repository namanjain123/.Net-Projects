using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.Graph.Models;
//using Microsoft.Graph.SecurityNamespace;
using Microsoft.Identity.Client;
using MongoDB.Driver;

namespace Webapi_automatestoreattendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        public static async Task<string> GetToken()
        {
            string clientId = "YourClientId";
            string clientSecret = "YourClientSecret";
            string tenantId = "YourTenantId";
            string[] scopes = new string[] { "https://graph.microsoft.com/.default", "Reports.Read.All" };

            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
                .Build();

            AuthenticationResult authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            string accessToken = authResult.AccessToken;

            return accessToken;
        }
        [HttpGet]
        public async Task<string> StoreAttendance(string meeting_id="",string user_id="")
        {
            var scopes = new[] { "User.Read", "OnlineMeetings.Read.All", "OnlineMeetings.ReadWrite.All" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = "common";

            // Value from app registration
            var clientId = "YOUR_CLIENT_ID";

            // using Azure.Identity;
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = clientId,
                TenantId = tenantId,
                // Callback function that receives the user prompt
                // Prompt contains the generated device code that user must
                // enter during the auth process in the browser
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
            var deviceCodeCredential = new DeviceCodeCredential(options);

            var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);
            var attendanceReports = await graphClient.Users[user_id].OnlineMeetings[meeting_id]
                .AttendanceReports.GetAsync();
            List<AttendanceRecord> attendanceReport = attendanceReports.Select(report => new AttendanceReport
            {
                UserPrincipalName = report.UserPrincipalName,
                DisplayName = report.DisplayName,
                JoinDateTime = report.JoinDateTime,
                LeaveDateTime = report.LeaveDateTime
            }).ToList();
            return attendanceReport.ToString();
        }
        [HttpPost]
        public async Task<string> StoreAttendance([FromBody] attenadancedata[] myObject)
        {
            string connectionString = "";
            string databaseName = "Meeting_bot";
            string collectionName = "Attendance";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<attenadancedata>(collectionName);
            await collection.InsertManyAsync(myObject);
            return "god job";
        }
    }
    public class attenadancedata
    {
        public string cloud { get; set; }
        public string kind { get;set; }
        public bool isAnonymous { get; set; }
        public string microsoftTeamsUserId { get; set; }
        public string rawId { get; set; }

    }
    public class AttendanceRecord
    {
        public string UserDisplayName { get; set; }
        public string UserPrincipalName { get; set; }
        public string JoinDateTime { get; set; }
        public string LeaveDateTime { get; set; }
    }


}



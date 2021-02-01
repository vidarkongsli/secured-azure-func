using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Linq;
using System.Net.Http;

namespace Company.Function
{
    public static class Profile
    {
        [FunctionName("Profile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var identity = req.HttpContext?.User?.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                var loginUri = $"/.auth/login/aad?post_login_redirect_url={Uri.EscapeDataString(req.Path + req.QueryString)}";
                log.LogInformation("Redirecting to {loginUri}", loginUri);
                return new RedirectResult(loginUri);
            }

            var accessToken = req.Headers.SingleOrDefault(h => h.Key == "X-MS-TOKEN-AAD-ACCESS-TOKEN").Value;
            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var result = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            return new OkObjectResult(data);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Company.Function
{
    public static class HelloWorld
    {
        [FunctionName("HelloWorld")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var identity = req.HttpContext?.User?.Identity as ClaimsIdentity;
            log.LogInformation("IsAuthenticated: {isAuthenticated}",identity?.IsAuthenticated);
            log.LogInformation("Identity name: {name}",identity?.Name);
            log.LogInformation("AuthenticationType: {authenticationType}", identity?.AuthenticationType);
            foreach (var claim in identity?.Claims) 
            {
                log.LogInformation("Claim: {type} : {value}", claim.Type, claim.Value);
            }
            return new OkObjectResult("Hello World");
        }
    }
}

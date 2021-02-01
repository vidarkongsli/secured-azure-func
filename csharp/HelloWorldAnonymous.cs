using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Company.Function
{
    public static class HelloWorldAnonymous
    {
        [FunctionName("HelloWorldAnonymous")]
        public static IActionResult Run(
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

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Utilites
{
    public class SSLBypassMiddleware
    {
        private readonly RequestDelegate _next;

        public SSLBypassMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            await _next.Invoke(context);
        }
    }
}

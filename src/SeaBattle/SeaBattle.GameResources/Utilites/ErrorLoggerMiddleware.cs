using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SeaBattle.GameResources.Utilites
{
    public class ErrorLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorLoggerMiddleware> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorLoggerMiddleware(RequestDelegate next, ILogger<ErrorLoggerMiddleware> logger, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception error)
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    _logger.LogDebug(error, "Unhandled exception");
                }
                else
                {
                    _logger.LogError(error, "Unhandled exception");
                }

                var response = context.Response;
                response.ContentType = "application/json";

                StringBuilder message = new StringBuilder();

                BuildMessage(error, message);

                var responseModel = ApiResponse<string>.Fail(message.ToString());

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonConvert.SerializeObject(responseModel);

                await response.WriteAsync(result);
            }
        }

        private void BuildMessage(Exception error, StringBuilder message)
        {
            message.AppendLine(error.Message);

            if (error.Data.Count > 0)
            {
                message.AppendLine("Data:");

                foreach (var key in error.Data.Keys)
                {
                    message.AppendLine($"{key}: {error.Data[key]}");
                }
            }

            if (error.InnerException != null)
            {
                message.AppendLine("Inner exception:");

                BuildMessage(error.InnerException, message);
            }
        }
    }
}

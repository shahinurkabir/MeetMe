using FluentValidation;
using MeetMe.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeetMe.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                logger.LogError(ex, ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                title = GetTitle(exception),
                status = statusCode,
                traceId = httpContext.TraceIdentifier,
                errors = GetErrors(exception),
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                CustomException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        private static string GetTitle(Exception exception) =>
            exception switch
            {
                ValidationException => "One or more validation errors occurred.",
                CustomException => string.IsNullOrEmpty(exception.Message) ? "Application Processing Error." : exception.Message,
                _ => "Server Error"
            };
        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            if (exception is ValidationException validationException)
            {
                foreach (var propertyGroupItem in validationException.Errors.GroupBy(e => e.PropertyName))
                {
                    var propertyName = propertyGroupItem.Key;
                    string[] propErrors = propertyGroupItem.ToList().Select(e => e.ErrorMessage).ToArray();
                    errors.Add(propertyName, propErrors);

                }
            }
            return errors;
        }
    }
}


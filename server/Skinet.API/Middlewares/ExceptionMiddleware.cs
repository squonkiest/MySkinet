using System.Net;
using System.Text.Json;
using Skinet.API.Errors;

namespace Skinet.API.Middlewares
{
    public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
    {
        private static readonly JsonSerializerOptions s_options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ApiErrorResponse response = env.IsDevelopment()
                ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var json = JsonSerializer.Serialize(response, s_options);

            return context.Response.WriteAsync(json);
        }
    }
}

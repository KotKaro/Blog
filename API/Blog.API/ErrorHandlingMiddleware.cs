using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Application.Mappers.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Blog.API
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            if (ex.GetType() == typeof(LoginException) || ex.GetType() == typeof(TokenInvalidException))
            {
                code = HttpStatusCode.Unauthorized;
            }

            var result = System.Text.Json.JsonSerializer.Serialize(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Security.Authentication;

namespace ICEDT.API.Middleware
{
    public class WrapResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public WrapResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            try
            {
                await _next(context);

                if (context.Response.StatusCode == 404)
                    throw new ArgumentException("The URL is not specified or invalid.");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, originalBodyStream);
                return;
            }

            context.Response.Body = originalBodyStream;
            newBodyStream.Seek(0, SeekOrigin.Begin);

            if (IsFileType(context.Response.ContentType))
            {
                context.Response.ContentLength = newBodyStream.Length;
                newBodyStream.Seek(0, SeekOrigin.Begin);
                await newBodyStream.CopyToAsync(context.Response.Body);
            }
            else
            {
                var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                var response = new Response();

                try
                {
                    response.Result = string.IsNullOrEmpty(responseBody) ? null : JsonSerializer.Deserialize<object>(responseBody);
                }
                catch
                {
                    response.Result = responseBody;
                }

                var wrappedResponseBody = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(wrappedResponseBody);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
        {
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                UnauthorizedAccessException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status403Forbidden
                ),
                AuthenticationException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status401Unauthorized
                ),
                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                ArgumentException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status404NotFound
                ),
                ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status404NotFound
                ),
                DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 2601 => (
                    "Duplicate sequence order detected.",
                    "DuplicateSequenceOrderException",
                    StatusCodes.Status400BadRequest
                ),
                DbUpdateException => (
                    "A database update error occurred.",
                    "DbUpdateException",
                    StatusCodes.Status400BadRequest
                ),
                _ => (
                    "An unexpected error occurred.",
                    exception.GetType().Name,
                    StatusCodes.Status500InternalServerError
                )
            };

            var extensions = new Dictionary<string, object?>
            {
                { "traceId", context.TraceIdentifier }
            };

            if (exception is ValidationException validationException)
            {
                extensions.Add("ValidationErrors", validationException.Errors);
            }

            var error = new Error(details.Title, details.Detail, details.StatusCode)
            {
                Extensions = extensions
            };

            context.Response.Body = originalBodyStream;
            var wrappedResponseBody = JsonSerializer.Serialize(new Response(null, true, error));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = details.StatusCode;
            await context.Response.WriteAsync(wrappedResponseBody);
        }

        private bool IsFileType(string? contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;

            var fileTypes = new List<string>
            {
                "application/pdf",
                "image/jpeg",
                "image/png",
                "image/gif"
            };

            return fileTypes.Contains(contentType);
        }
    }




    public static class WrapResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseWrapResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WrapResponseMiddleware>();
        }
    }
}
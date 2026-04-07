using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.Exceptions;
using CeylonHire.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CeylonHire.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string message = exception.Message;
            HttpStatusCode status;

            switch (exception)
            {
                case DomainException:
                    status = HttpStatusCode.BadRequest;
                    break;

                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    break;

                case DuplicateEmailException:
                    status = HttpStatusCode.Conflict;
                    break;

                case Application.Exceptions.BadRequestException:
                    status = HttpStatusCode.BadRequest;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new ApiResponse<string>
            {
                Success = false,
                Message = message,
            };

            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(payload);
        }
    }
}

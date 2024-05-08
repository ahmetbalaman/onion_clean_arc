using FluentValidation;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
namespace OnionArcAndAll.Application.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = GetStatusCode(exception);
            if(exception.GetType() == typeof(ValidationException))
                return httpContext.Response.WriteAsync(new ExceptionModel()
                {
                    Errors = ((ValidationException)exception).Errors.Select(x=> x.ErrorMessage),
                    StatusCode = StatusCodes.Status400BadRequest

                }.ToString());


            List<string> errors = new()
            {
                $"Hata Mesajı : {exception.Message}",
             //   $"Mesaj Açıklaması : {exception.InnerException}".ToString()
            };
            return httpContext.Response.WriteAsync(new ExceptionModel
            {
                StatusCode = httpContext.Response.StatusCode,
                Errors = errors
            }.ToString());
        }

        private static int GetStatusCode(Exception exception) => exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

    }

}

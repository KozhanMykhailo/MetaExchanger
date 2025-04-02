using MetaExchanger.Contracts.Responses;
using FluentValidation;

namespace MetaExchanger.Api.Mapping
{
    /// <summary>
    /// Handles validation errors.
    /// </summary>
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                var validationFailureResponce = new ValidationFailureResponce
                {
                    Errors = ex.Errors.Select(x => new ValidationResponce
                    {
                        PropertyName = x.PropertyName,
                        Message = x.ErrorMessage,
                    })
                };

                await context.Response.WriteAsJsonAsync(validationFailureResponce);
            }
        }
    }
}
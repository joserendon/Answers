using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Middlewares
{
    public class DuplicateKeyExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var innerException = dbUpdateException.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("Ya existe un registro con el mismo nombre.");
                }
            }
        }
    }

}

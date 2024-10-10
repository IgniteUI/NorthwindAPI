using System.Text.RegularExpressions;

namespace NorthwindCRUD.Middlewares
{
    public class TenantHeaderValidationMiddleware
    {
        private const string TenantHeaderKey = "X-Tenant-ID";

        private readonly RequestDelegate _next;

        public TenantHeaderValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantHeader = context.Request.Headers[TenantHeaderKey].FirstOrDefault();

            if (tenantHeader != null && !IsTenantValid(tenantHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"Invalid format for Header {TenantHeaderKey}");
                return;
            }

            await _next(context);
        }

        private bool IsTenantValid(string tenantId)
        {
            return Regex.IsMatch(tenantId, "^[A-Za-z0-9-_]{0,40}$");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace ProductManagement.API.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _userAgentHeaderKey = "User-Agent";
        private const string _correlationIdHeaderKey = "X-Correlation-ID";

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string userAgentFromHeader = context.Request.Headers.ContainsKey(_userAgentHeaderKey) ? context.Request.Headers[_userAgentHeaderKey].ToString() : null;
            string correlationIdFromHeader = context.Request.Headers.ContainsKey(_correlationIdHeaderKey) ? context.Request.Headers[_correlationIdHeaderKey].ToString() : null;

            LogContext.PushProperty(_userAgentHeaderKey, userAgentFromHeader);
            LogContext.PushProperty(_correlationIdHeaderKey, correlationIdFromHeader);

            await _next(context);
        }
    }
}

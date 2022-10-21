using Puissance4.Business.Interfaces;

namespace Puissance4.API.Middlewares
{
    public class HubJwtMiddleware
    {
        private readonly RequestDelegate _next;

        public HubJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            string token = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                var user = jwtService.ValidateToken(token);
                if (user != null)
                    context.User = user;
            }
            await _next.Invoke(context);
        }
    }
}

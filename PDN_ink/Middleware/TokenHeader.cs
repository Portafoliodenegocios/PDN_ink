using Pdnink_Coremvc.Helpers;

namespace Pdnink.Middleware
{

    public class TokenHeader
    {
        private readonly RequestDelegate _next;

        public TokenHeader(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Constants constants)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = constants.Token;

                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Append("Authorization", token);
                }
            }

            await _next(context);
        }
       


    }

    public static class TokenHeaderExtensions
    {
        public static IApplicationBuilder UseTokenHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenHeader>();
        }
    }

}

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProductiveBoard.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Redirect("/");
        }
    }
}

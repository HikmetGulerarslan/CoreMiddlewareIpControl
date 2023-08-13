# CoreMiddlewareIpControl
API Projelerinizde kullanabileceğiniz IP bazlı sınırlama koymamızı sağlayan Middleware Yapısı

IPSafeMiddleWare.cs

using System.Net;

namespace IpControl.Middleware
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly string[] _ipBlackList = { "127.0.0.1","::1"};
        public IPSafeMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var requestIpAdress = context.Connection.RemoteIpAddress;
            var isWhiteList = _ipBlackList.Where(x => IPAddress.Parse(x).Equals(requestIpAdress)).Any();
            if (!isWhiteList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next(context);
        }
    }
}

Program.cs

using IpControl.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
var app = builder.Build();

app.UseMiddleware<IPSafeMiddleWare>();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());


app.Run();

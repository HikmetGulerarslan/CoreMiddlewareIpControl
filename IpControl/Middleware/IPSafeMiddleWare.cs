using System.Net;

namespace IpControl.Middleware
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly string[] _ipList = { "127.0.0.1","::1"};
        public IPSafeMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var requestIpAdress = context.Connection.RemoteIpAddress;
            if (!_ipList.Where(x => IPAddress.Parse(x).Equals(requestIpAdress)).Any())
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next(context);
        }
    }
}

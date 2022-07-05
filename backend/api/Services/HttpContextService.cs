using Microsoft.AspNetCore.Authentication;

namespace api.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<string> GetToken()
        {
            return await _contextAccessor.HttpContext.GetTokenAsync("access_token");
        }
    }
}

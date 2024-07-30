

using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;

namespace WebApp.StockApp.MiddleWares
{
    public class ValidateUserSession
    {
        protected readonly IHttpContextAccessor _httpContextAccesor;
        public ValidateUserSession(IHttpContextAccessor _httpContextAccesor)
        {
            this._httpContextAccesor = _httpContextAccesor;
        }
        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccesor.HttpContext.Session.Get<AuthenticationResponse>("user");
            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}

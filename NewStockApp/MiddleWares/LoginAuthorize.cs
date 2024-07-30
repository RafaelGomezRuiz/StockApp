using DatabaseFirstExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.StockApp.MiddleWares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        protected readonly ValidateUserSession _validateUserSession;
        public LoginAuthorize(ValidateUserSession _validateUserSession)
        {
            this._validateUserSession = _validateUserSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_validateUserSession.HasUser())
            {
                var controller=(UserController)context.Controller;
                context.Result = controller.RedirectToAction("index", "home");
            }
            else
            {
                await next();
            }
        }
    }
}

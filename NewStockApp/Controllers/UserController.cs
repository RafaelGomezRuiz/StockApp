using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Users;
using StockApp.Core.Application.Helpers;
using StockApp.Core.Domain.Entities;
using WebApp.StockApp.MiddleWares;
using StockApp.Core.Application.Dtos.Account;

namespace DatabaseFirstExample.Controllers
{
    public class UserController : Controller
    {
        protected readonly IUserService _userService;

        public UserController(IUserService _userService)
        {
            this._userService= _userService;
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Index()
        {
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel LoginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(LoginVm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(LoginVm);
            if (userVm !=null && userVm.HasError!=true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                LoginVm.HasError = true;
                LoginVm.ErrorDescription = userVm.ErrorDescription;
                return View(LoginVm);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Register()
        {
            return View(new SaveUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SaveUserViewModel UserVm)
        {
            if (!ModelState.IsValid)
            {
                return View(UserVm);
            }
            var origin = Request.Headers["origin"];
            RegisterResponse response= await _userService.RegisterAsync(UserVm,origin);
            if (response.HasError)
            {
                UserVm.HasError=response.HasError;
                UserVm.ErrorDescription = response.ErrorDescription;
                return View(UserVm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            string response=await _userService.ConfirmEmailAsync(userId,token);
            return View("ConfirmEmail",response);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.ErrorDescription = response.ErrorDescription;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        { 
            return View(vm);
        }
        
        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword",vm);
            }
            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.ErrorDescription = response.ErrorDescription;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}

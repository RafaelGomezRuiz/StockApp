using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace StockApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Authentication User Controller")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("autenticate")]
        [SwaggerOperation(
            Summary = "Authentica a user",
            Description = "Validate the credentials"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await accountService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "register an user",
            Description = "Validate the necessary parameters to create an user"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await accountService.RegisterBasicUserAsync(request, origin));
        }

        [HttpGet("confirm-email")]
        [SwaggerOperation(
            Summary = "Confirm Email",
            Description = "This endpoint is automatically set by the app suring the proccess"
        )]
        public async Task<IActionResult> ConfirmAccountAsync([FromQuery]ConfirmAccount confirmAccountDto)
        {
            return Ok(await accountService.ConfirmAccountAsync(confirmAccountDto.UserId,confirmAccountDto.Token));
        }

        [HttpPost("forgot-password")]
        [SwaggerOperation(
            Summary = "forgot password ",
            Description = "send the necessary values to do the changes"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await accountService.ForgotPasswordAsync(request,origin));
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(
            Summary = "reset password ",
            Description = "after validate the values here we already changed the password"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await accountService.ResetPasswordAsync(request));
        }

    }
}

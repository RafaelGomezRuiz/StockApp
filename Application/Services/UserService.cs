using AutoMapper;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Users;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        protected readonly IMapper _mapper;


        public UserService(IAccountService _accountService, IMapper _mapper)
        {
            this._accountService = _accountService;
            this._mapper = _mapper;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            RegisterResponse registerResponse = await _accountService.RegisterBasicUserAsync(registerRequest, origin);
            return registerResponse;

        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest authenticationRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse authenticationResponse = await _accountService.AuthenticateAsync(authenticationRequest);
            return authenticationResponse;
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel forgotPasswordVm, string origin)
        {
            ForgotPasswordRequest forgotPasswordRequest = _mapper.Map<ForgotPasswordRequest>(forgotPasswordVm);
            ForgotPasswordResponse forgotPasswordResponse = await _accountService.ForgotPasswordAsync(forgotPasswordRequest, origin);
            return forgotPasswordResponse;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel resetPasswordVm)
        {
            ResetPasswordRequest resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(resetPasswordVm);
            return await _accountService.ResetPasswordAsync(resetPasswordRequest);
        }


    }
}

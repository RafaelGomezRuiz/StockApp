using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Dtos.Email;
using StockApp.Core.Application.Dtos.Token;
using StockApp.Core.Application.Enums;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Domain.Settings;
using StockApp.Infraestructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockApp.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings JWTSettings;


        public AccountService(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            IEmailService _emailService,
            IOptions<JWTSettings> JWTSettings
            )
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._emailService = _emailService;
            this.JWTSettings = JWTSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse authenticationResponse = new();

            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                authenticationResponse.HasError = true;
                authenticationResponse.ErrorDescription = $"No acounts with this {request.Email} address";
                return authenticationResponse;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                authenticationResponse.HasError = true;
                authenticationResponse.ErrorDescription = $"Invalid credentials for {request.Email}";
                return authenticationResponse;
            }

            if (!user.EmailConfirmed)
            {
                authenticationResponse.HasError = true;
                authenticationResponse.ErrorDescription = $"Acount not confirmed for {request.Email}";
                return authenticationResponse;
            }

            authenticationResponse.Id = user.Id;
            authenticationResponse.Email = user.Email;
            authenticationResponse.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            authenticationResponse.Roles = rolesList.ToList();
            authenticationResponse.IsVerified = user.EmailConfirmed;
            
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            authenticationResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            
            var refreshTokenObject = GenerateRefreshToken();
            authenticationResponse.RefreshToken = refreshTokenObject.Token;
            
            return authenticationResponse;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var UserWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (UserWithSameUserName != null)
            {
                response.HasError = true;
                response.ErrorDescription = $"this UserName '{request.UserName}' is alreadyTaken";
                return response;
            }

            var UserWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (UserWithSameEmail != null)
            {
                response.HasError = true;
                response.ErrorDescription = $"this Email '{request.Email}' is already registered";
                return response;
            }

            ApplicationUser user = new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };

            var userCreated = await _userManager.CreateAsync(user, request.Password);
            if (userCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.BASIC.ToString());
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new EmailRequest
                {
                    To = user.Email,
                    Body = $"Please confirm your acount visisting this link: {verificationUri}",
                    Subject = "Confirm registration"
                });
            }
            else
            {
                response.HasError = true;
                response.ErrorDescription = $"An error has ocurred trying to save the user";
                return response;
            }

            return response;
        }
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. Yuo can use the app";
            }
            else
            {
                return $"An error occured while confirming {user.Email}";
            }
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.ErrorDescription = $"No users registered with this {request.Email} email";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new EmailRequest()
            {
                To = request.Email,
                Body = $"Please reset your account visisting this Url: {verificationUri}",
                Subject = "Reset Password"
            });

            return response;
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.ErrorDescription = $"No accounts registered with this {request.Email} email";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorDescription = $"No accounts registered with this {request.Email} email";
                return response;
            }

            return response;
        }

        #region privates
        private async Task<JwtSecurityToken> GenerateJWToken( ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            foreach(var role in userRoles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId",user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.Key));

            var signinCredentials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer : JWTSettings.Issuer,
                audience : JWTSettings.Audience,
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(JWTSettings.DurationInMinutes),
                signingCredentials : signinCredentials
                );

            return jwtSecurityToken;
        } 

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Token= RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new Byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);
            return verificationUri;
        }
        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);
            return verificationUri;
        }
        #endregion

    }
}

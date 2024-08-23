

using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parameters to reset password
    /// </summary>
    public class ResetPasswordRequest
    {

        [SwaggerParameter(Description = "user email to send the new password")]
        public string? Email { get; set; }
        
        [SwaggerParameter(Description = "the token to validate that is secure to change")]
        public string? Token { get; set; }
        
        [SwaggerParameter(Description = "The new password")]
        public string? Password { get; set; }
        
        [SwaggerParameter(Description = "Confirm the new password")]
        public string? ConfirmPassword { get; set; }

    }
}



using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// parameters to init the proccess of fotgot password
    /// </summary>
    public class ForgotPasswordRequest
    {
        [SwaggerParameter(Description = "user email to request a change of password")]
        public string? Email { get; set; }
    }
}

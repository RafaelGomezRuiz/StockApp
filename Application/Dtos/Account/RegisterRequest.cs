using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.Dtos.Account
{
    /// <summary>
    /// Parameters to register a user
    /// </summary>
    public class RegisterRequest
    {
        [SwaggerParameter(Description = "resgiter name of the user")]
        public string? FirstName { get; set; }
        
        [SwaggerParameter(Description = "lastname of the user")]
        public string? LastName { get; set; }
        
        [SwaggerParameter(Description = "user email address to validate")]
        public string? Email { get; set; }
        
        [SwaggerParameter(Description = "we wait a valid userName")]
        public string? UserName { get; set; }
        
        [SwaggerParameter(Description = "we have a password with the necessary requirement")]
        public string? Password { get; set; }
        
        [SwaggerParameter(Description = "the same password to validate")]
        public string? ConfirmPassword { get; set; }
        
        [SwaggerParameter(Description = "user phone number")]
        public string? Phone { get; set; }
    }
}

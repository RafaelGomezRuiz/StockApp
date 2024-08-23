using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.Dtos.Account
{

    /// <summary>
    /// parameters to authenticate the user
    /// </summary>
    public class AuthenticationRequest
    {
        [SwaggerParameter(Description ="user email to sign in")]
        public string? Email { get; set; }
        [SwaggerParameter(Description = "user password to sign in")]
        public string? Password { get; set; }

    }
}

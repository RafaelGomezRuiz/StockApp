﻿using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.ViewModels.Users
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Debe colocar su email")]
        [DataType(DataType.Text)]
        public string? Email { get; set; }
        public bool HasError { get; set; }
        public string? ErrorDescription { get; set; }
    }
}

﻿using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductsQuantity { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }

    }
}

using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        //Parallel automappign
        public CategoryViewModel Category { get; set; }
        public string UserId { get; set; }
    }
}

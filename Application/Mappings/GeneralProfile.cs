using AutoMapper;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Application.ViewModels.Users;
using StockApp.Core.Domain.Entities;
namespace StockApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(destino => destino.CategoryName, otp => otp.Ignore())
                .ReverseMap()
                .ForMember(destino => destino.Created, otp => otp.Ignore())
                .ForMember(destino => destino.CreatedBy, otp => otp.Ignore())
                .ForMember(destino => destino.LastModified, otp => otp.Ignore())
                .ForMember(destino => destino.LastModifiedBy, otp => otp.Ignore());
            
            CreateMap<Product, SaveProductViewModel>()
                .ForMember(destino => destino.Categories, otp => otp.Ignore())
                .ForMember(destino => destino.File, otp => otp.Ignore())
                .ReverseMap()
                .ForMember(destino => destino.Created, otp => otp.Ignore())
                .ForMember(destino => destino.CreatedBy, otp => otp.Ignore())
                .ForMember(destino => destino.LastModified, otp => otp.Ignore())
                .ForMember(destino => destino.LastModifiedBy, otp => otp.Ignore())
                .ForMember(destino => destino.Category, otp => otp.Ignore());
            
            CreateMap<Category, CategoryViewModel>()
                .ForMember(destino => destino.ProductsQuantity, otp => otp.Ignore())
                .ReverseMap()
                .ForMember(destino => destino.Created, otp => otp.Ignore())
                .ForMember(destino => destino.CreatedBy, otp => otp.Ignore())
                .ForMember(destino => destino.LastModified, otp => otp.Ignore())
                .ForMember(destino => destino.LastModifiedBy, otp => otp.Ignore())
                .ForMember(destino => destino.Products, otp => otp.Ignore());

            CreateMap<Category, SaveCategoryViewModel>()
                .ReverseMap()
                .ForMember(destino => destino.Created, otp => otp.Ignore())
                .ForMember(destino => destino.CreatedBy, otp => otp.Ignore())
                .ForMember(destino => destino.LastModified, otp => otp.Ignore())
                .ForMember(destino => destino.LastModifiedBy, otp => otp.Ignore())
                .ForMember(destino => destino.Products, otp => otp.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(destino => destino.HasError, otp => otp.Ignore())
                .ForMember(destino => destino.ErrorDescription, otp => otp.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(destino => destino.HasError, otp => otp.Ignore())
                .ForMember(destino => destino.ErrorDescription, otp => otp.Ignore())
                .ReverseMap();
            
            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(destino => destino.HasError, otp => otp.Ignore())
                .ForMember(destino => destino.ErrorDescription, otp => otp.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(destino => destino.HasError, otp => otp.Ignore())
                .ForMember(destino => destino.ErrorDescription, otp => otp.Ignore())
                .ReverseMap();
        }
    }
}

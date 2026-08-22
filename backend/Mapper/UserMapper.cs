using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace backend.Mapper;

using AutoMapper;
using backend.model;
using backend.Model;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<Product, ProductSalesResponseDTO>();
        CreateMap<Product, ProductInventoryResponseDTO>();

        CreateMap<ProductInventoryRequestDTO, Product>();

        CreateMap<ReceiptItem, ReceiptItemResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
        CreateMap<ReceiptItemRequest, ReceiptItem>();
        CreateMap<Receipt, ReceiptResponse>();

        CreateMap<InventorySettingsPatchRequest, InventorySetting>()
            .ForAllMembers(options => options.Condition((source, destination, sourceMember) => sourceMember != null));
    }
}

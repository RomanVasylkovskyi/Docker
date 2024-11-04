using AutoMapper;
using System.Globalization;
using WebHalk.Data.Entities;
using WebHalk.Data.Entities.Identity;
using WebHalk.Models.Account;
using WebHalk.Models.Categories;
using WebHalk.Models.Products;

namespace WebHalk.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryEntity, WebHalk.Areas.Admin.Models.Category.CategoryItemViewModel>();

            CreateMap<CategoryEntity, WebHalk.Areas.Admin.Models.Category.CategoryEditViewModel>();
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x=>x.Images, opt=>opt.MapFrom(x=>x.ProductImages.Select(p=>p.Image).ToArray()));

            CreateMap<ProductEntity, WebHalk.Areas.Admin.Models.Products.ProductItemViewModel>()
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.Image).ToArray()));
           
          
            CreateMap<WebHalk.Areas.Admin.Models.Products.ProductEditViewModel, ProductEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Price, opt => opt.MapFrom(x => Decimal.Parse(x.Price, new CultureInfo("uk-UA"))));

            CreateMap<UserEntity, ProfileViewModel>()
                .ForMember(x=>x.FullName, opt=>opt.MapFrom(x=>$"{x.LastName} {x.FirstName}"));


            CreateMap<ProductEntity, WebHalk.Areas.Admin.Models.Products.ProductItemViewModel>()
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.Image).ToArray()));

            
            CreateMap<ProductEntity, WebHalk.Areas.Admin.Models.Products.ProductEditViewModel>()
              .ForMember(x => x.Images, opt =>
              opt.MapFrom(src => src.ProductImages
              .Select(pi => new WebHalk.Areas.Admin.Models.Products.ProductImageViewModel
              {
                  Id = pi.Id,
                  Name = "/images/" + pi.Image,
                  Priority = pi.Priotity
              }).ToList()))
                    .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price.ToString(new CultureInfo("uk-UA"))));
        }
    }
}

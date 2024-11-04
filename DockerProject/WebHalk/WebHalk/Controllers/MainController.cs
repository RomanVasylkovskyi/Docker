using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebHalk.Data;
using WebHalk.Data.Entities;
using WebHalk.Models.Categories;

namespace WebHalk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext _hulkDbContext;
        private readonly IMapper _mapper;

        public MainController(HulkDbContext hulkDbContext, IMapper mapper)
        {
            _hulkDbContext = hulkDbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var list = _hulkDbContext.Categories
                .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
                //.Select(x=> new CategoryItemViewModel
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Image = x.Image
                //})
                .ToList();
            return View(list);
        }
    }
}

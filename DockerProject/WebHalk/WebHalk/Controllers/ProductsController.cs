using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using WebHalk.Data;
using WebHalk.Data.Entities;
using WebHalk.Models.Products;

namespace WebHalk.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HulkDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(HulkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index(ProductSearchViewModel search)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));

            int count = query.Count();
            int page = search.Page ?? 1;
            int pageSize = search.PageSize;

            query = query.OrderBy(x=>x.Id)
                .Skip(page-1)
                .Take(pageSize);

            var list = query
                  .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                  .ToList() ?? throw new Exception("Failed to get products");

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            ProductHomeViewModel model = new ProductHomeViewModel
            {
                Search = search,
                Products = list,
                Count = count,
                Pagination = new PaginationViewModel
                {
                    PageSize = pageSize,
                    TotalItems = count
                }
            };

            Console.WriteLine("RunTime ProductsController Index" + elapsedTime);

            return View(model);
        }

    }
}

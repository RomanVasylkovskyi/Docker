using ApiStore.Data;
using ApiStore.Data.Entities;
using ApiStore.Interfaces;
using ApiStore.Models.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ApiStoreDbContext context, IImageHulk imageHulk,
        IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var list = context.Products
                .ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
        {
            var entity = mapper.Map<ProductEntity>(model);
            context.Products.Add(entity);
            context.SaveChanges();

            if (model.Images != null)
            {
                var p = 1;
                foreach (var image in model.Images)
                {
                    var pi = new ProductImageEntity
                    {
                        Image = await imageHulk.Save(image),
                        Priority = p,
                        ProductId = entity.Id
                    };
                    p++;
                    context.ProductImages.Add(pi);
                    await context.SaveChangesAsync();
                }
            }
            return Created();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = context.Products
                .Include(x=>x.ProductImages)
                .SingleOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            if (product.ProductImages != null)
                foreach (var p in product.ProductImages)
                    imageHulk.Delete(p.Image);

            context.Products.Remove(product);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await context.Products
                .ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductEditViewModel model)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            mapper.Map(model, product);

            if (model.PreviousImages != null)
            {
                foreach (var prevImage in model.PreviousImages)
                {
                    var existingImage = product.ProductImages
                        .FirstOrDefault(img => img.Id == prevImage.Id);

                    if (existingImage != null)
                        existingImage.Priority = prevImage.Priority;
                }
            }

            ///Видаляємо не потрібні фото
            if (model.ImagesIds != null)
            {
                var imagesToDelete = context.ProductImages
                    .Where(img => model.ImagesIds.Contains(img.Id))
                    .ToList();

                foreach (var img in imagesToDelete)
                {
                    imageHulk.Delete(img.Image);
                    context.ProductImages.Remove(img);
                }
            }

            ///Нові фотки для товару - я де пріорітет - мабуть забулися
            if (model.NewImages != null)
            {
                foreach (var img in model.NewImages)
                {
                    if (img != null)
                    {
                        var imagePath = await imageHulk.Save(img);
                        context.ProductImages.Add(new ProductImageEntity
                        {
                            Image = imagePath,
                            ProductId = product.Id,
                        });
                    }
                }
            }
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

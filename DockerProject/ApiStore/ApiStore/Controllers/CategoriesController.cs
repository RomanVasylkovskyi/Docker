using ApiStore.Data;
using ApiStore.Data.Entities;
using ApiStore.Interfaces;
using ApiStore.Models.Category;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ApiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(
        ApiStoreDbContext context, IImageHulk imageHulk,
        IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var list = context.Categories
                .ProjectTo<CategoryItemViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            var imageName = await imageHulk.Save(model.Image);
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = imageName;
            context.Categories.Add(entity);
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = context.Categories.SingleOrDefault(x => x.Id == id);
            if (entity == null)
                return NotFound();
            if (!string.IsNullOrEmpty(entity.Image))
                imageHulk.Delete(entity.Image);
            context.Categories.Remove(entity);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model)
        {
            if (model == null) return NotFound();
            var category = context.Categories.SingleOrDefault(x => x.Id == model.Id);
            category = mapper.Map(model, category);
            if (model.Image != null)
            {
                imageHulk.Delete(category.Image);
                string fname = await imageHulk.Save(model.Image);
                category.Image = fname;
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = context.Categories
                .ProjectTo<CategoryItemViewModel>(mapper.ConfigurationProvider)
                .SingleOrDefault(x=>x.Id==id);
            return Ok(item);
        }
    }
}

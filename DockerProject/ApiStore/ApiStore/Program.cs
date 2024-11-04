using ApiStore.Constants;
using ApiStore.Data;
using ApiStore.Data.Entities;
using ApiStore.Data.Entities.Identity;
using ApiStore.Interfaces;
using ApiStore.Mapper;
using ApiStore.Services;
using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiStoreDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApiStoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(AppMapProfile));
builder.Services.AddScoped<IImageHulk, ImageHulk>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var singinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<String>("JwtSecretKey")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = singinKey,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
builder.Services.AddSwaggerGen(c =>
{
    var fileDoc = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
    c.IncludeXmlComments(fileDoc);
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Jwt Auth header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Id="Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});

var app = builder.Build();

app.UseCors(opt =>
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string imagesDirPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"]);

if (!Directory.Exists(imagesDirPath))
{
    Directory.CreateDirectory(imagesDirPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesDirPath),
    RequestPath = "/images"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


#pragma warning restore ASP0014 // Suggest using top level route registrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiStoreDbContext>();
    var imageHulk = scope.ServiceProvider.GetRequiredService<IImageHulk>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    //dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    if(dbContext.Categories.Count()==0)
    {
        int number = 10;
        var list = new Faker("uk")
            .Commerce.Categories(number);
        foreach(var name in list)
        {
            string image = imageHulk.Save("https://picsum.photos/1200/800?category").Result;
            var cat = new CategoryEntity
            {
                Name = name,
                Description = new Faker("uk").Commerce.ProductDescription(),
                Image = image
            };
            dbContext.Categories.Add(cat);
            dbContext.SaveChanges();
        }
    }

    if (dbContext.Products.Count() == 0)
    {
        var categories = dbContext.Categories.ToList();

        var fakerProduct = new Faker<ProductEntity>("uk")
            .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
            .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
            .RuleFor(u => u.Category, (f, u) => f.PickRandom(categories));

        string url = "https://picsum.photos/1200/800?product";

        var products = fakerProduct.GenerateLazy(100);
        Random r = new Random();

        foreach (var product in products)
        {
            dbContext.Add(product);
            dbContext.SaveChanges();
            int imageCount = r.Next(3, 5);
            for (int i = 0; i < imageCount; i++)
            {
                var imageName = imageHulk.Save(url).Result;
                var imageProduct = new ProductImageEntity
                {
                    Product = product,
                    Image = imageName,
                    Priority = i
                };
                dbContext.Add(imageProduct);
                dbContext.SaveChanges();
            }
        }
    }

    // seed roles
    if (dbContext.Roles.Count() == 0)
    {
        var roles = new[]
        {
            new RoleEntity { Name = Roles.Admin },
            new RoleEntity { Name = Roles.User }
        };

        foreach (var role in roles)
        {
            var outcome = roleManager.CreateAsync(role).Result;
            if (!outcome.Succeeded) Console.WriteLine($"Failed to create role: {role.Name}");
        }
    }

    // seed users
    if (dbContext.Users.Count() == 0)
    {
        var users = new[]
        {
            new { User = new UserEntity { Firstname = "Tony", Lastname = "Stark", UserName = "admin@gmail.com", Email = "admin@gmail.com" }, Password = "admin1", Role = Roles.Admin },
            new { User = new UserEntity { Firstname = "Boba", Lastname = "Gray", UserName = "user@gmail.com", Email = "user@gmail.com" }, Password = "bobapass1", Role = Roles.User },
            new { User = new UserEntity { Firstname = "Biba", Lastname = "Undefined", UserName = "biba@gmail.com", Email = "biba@gmail.com" }, Password = "bibapass3", Role = Roles.User }
        };

        foreach (var i in users)
        {
            var outcome = userManager.CreateAsync(i.User, i.Password).Result;

            if (!outcome.Succeeded)
                Console.WriteLine($"Failed to create user: {i.User.UserName}");
            else
                outcome = userManager.AddToRoleAsync(i.User, i.Role).Result;
        }
    }
}

app.Run();

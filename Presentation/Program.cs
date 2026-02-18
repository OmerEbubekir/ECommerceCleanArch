using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//DbContext 
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure")));
// Scoped: Her HTTP isteði (Request) için yeni bir nesne oluþturur ve istek bitince siler.

//GenericRepository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//UnitWork 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Product servece
builder.Services.AddScoped<IProductService, ProductService>();

// Parantez içine MappingProfiles sýnýfýnýn bulunduðu Assembly'i (Application projesi) veriyoruz.
builder.Services.AddAutoMapper(typeof(Application.Helpers.MappingProfiles));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

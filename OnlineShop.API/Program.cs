using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Data;
using OnlineShop.API.Repo.Interfaces;
using OnlineShop.API.Repo.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IBuyer, BuyerRepo>();
builder.Services.AddScoped<IFeedback, FeedbackRepo>();
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped <ISeller, SellerRepo>();
builder.Services.AddDbContext<MyDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
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

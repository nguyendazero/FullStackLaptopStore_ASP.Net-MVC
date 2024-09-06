using DotNet.BookStore.Data;
using Microsoft.EntityFrameworkCore;
using DotNet.BookStore.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ILikeRatingService, LikeRatingService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

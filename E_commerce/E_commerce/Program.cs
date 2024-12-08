
using E_commerce.Models;
using E_commerce.Services;
using E_commerce.Interface;

namespace E_commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoDbContextSetting>
                (builder.Configuration.GetSection("E_commerceDatabase"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            builder.Services.AddSingleton(typeof(IDatabaseService<>), typeof(DatabaseService<>));

            builder.Services.AddSingleton<IProductService, ProductServices>();
            builder.Services.AddSingleton<ICategoryService, CategoryServices>();


            // Add services to the container.

            builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors("AllowAngularDev");
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
        }
    }
}
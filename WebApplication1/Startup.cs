using LearningPathDev.DatabaseContext;
using LearningPathDev.Interfaces;
using LearningPathDev.Mapper;
using LearningPathDev.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace LearningPathDev
{
    public class Startup
    {
        readonly string _MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: _MyAllowSpecificOrigins,
                    builder => builder.WithOrigins("http://localhost:3000")
                                                    .AllowAnyHeader()
                                                  .AllowAnyMethod());
            });
            services.AddCors();
            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(Mappers));
            services.AddControllers();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var connectionStrings = Configuration["ConnectionStrings:ProdConnection"];
            services.AddDbContext<ProductsContext>(o => o.UseSqlServer(connectionStrings));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

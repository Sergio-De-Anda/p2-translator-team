using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using P2Translator.Data;

namespace P2Translator.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if(connectionString == null)
            {
              Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "server=postgres_image;port=5432;database=postgres;username=postgres;password=postgres");
              connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            }            
            services.AddDbContext<P2TranslatorDbContext>(options => options.UseNpgsql(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // app.UseHttpsRedirection();

            app.UseRouting();
            UpdateDatabase(app);
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<P2TranslatorDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}

using GISA.BPM.Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GISA.BPM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            //services.AddClaims();
            services.ConfigureCors(Configuration);
            services.AddAutoMapper();
            services.AddSwaggerDocumentation(Configuration);
            services.AddApiVersion();
            services.AddHealthChecks();
            services.AddResponseCompression();
            services.AddHttpContextAccessor();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });

            app.UseAppCors();
            app.UseHttpsRedirection();
            app.UseHealthChecks("/health");
            app.AddSwaggerApplication(versionProvider);
            app.UseApiVersioning();
            app.UseRouting();
            app.UseAuthorization();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}

using GISA.BPM.Application.Contracts;
using GISA.BPM.Application.Mappings;
using GISA.BPM.Application.Notifications;
using GISA.BPM.Application.Services;
using GISA.BPM.Domain.Contracts;
using GISA.BPM.HttpContext.Shared.Contracts;
using GISA.BPM.HttpContext.Shared.Models;
using GISA.BPM.Infrastructure.Data.Configurations;
using GISA.BPM.Infrastructure.Data.Contracts;
using GISA.BPM.Infrastructure.Data.Repositories;
using GISA.BPM.Infrastructure.Storage.Configurations;
using GISA.BPM.Infrastructure.Storage.Contracts;
using GISA.BPM.Infrastructure.Storage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GISA.BPM.Infrastructure.IoC
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddSingleton<ICloudConfiguration, DynamoDBConfiguration>();
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddSingleton<IStorageConfiguration, S3Configuration>();
            services.AddScoped<IStorageRepository, S3StorageRepository>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddTransient<IClaimContext, ClaimContext>();
            return services;
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(WorkflowMapping));
        }

        public static void AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GISA BPM API",
                    Contact = new OpenApiContact
                    {
                        Name = "Luis Paulo Nakamura"
                    }
                });
            });
        }

        public static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void AddSwaggerApplication(this IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                foreach (var versionDescription in versionProvider.ApiVersionDescriptions)
                    swaggerUiOptions.SwaggerEndpoint($"../swagger/{versionDescription.GroupName}/swagger.json", versionDescription.GroupName.ToUpperInvariant());

                swaggerUiOptions.DocExpansion(DocExpansion.List);
            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options
                    .AddPolicy("CORS", option =>
                    {
                        option
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                       .WithOrigins(configuration.GetSection("ALLOWED_CORS").Get<string[]>());
                    }));
        }

        public static void UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("CORS");
        }

        public static void AddClaims(this IServiceCollection services)
        {
            //services.AddTransient<IClaimContext>(serviceProvider =>
            //{
            //    var context = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            //    var token = context.HttpContext.Request.Headers["Authorization"];

            //    if (string.IsNullOrEmpty(token))
            //        throw new Exception("Token is required");

            //    var handler = new JwtSecurityTokenHandler();
            //    var jwtSecurityToken = handler.ReadJwtToken(token);

            //    var userNameClaim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "username");

            //    return new ClaimContext() { UserName = userNameClaim.Value };
            //});
        }
    }
}

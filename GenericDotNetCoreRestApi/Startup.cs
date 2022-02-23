using GenericDotNetCoreRestApi.Extension;
using GenericDotNetCoreRestApi.Model.Context;
using GenericDotNetCoreRestApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDataProtection();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                var tokenOptions = Configuration.GetSection(nameof(TokenOptions)).Get<TokenOptions>();
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = tokenOptions.GetSymmetricSecurityKey()
                };
            });


            _ = services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3.1", new OpenApiInfo()
                {
                    Version = "v3.1",
                    Title = "UniSource API Service",
                    Description = "Client API Service",
                    Contact = new OpenApiContact
                    {
                        Name = "UniSource",
                        Url = new Uri("https://unisource.co.za/")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Specify the authorization token.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer",

                });

                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                  {
                    {securityScheme, new string[] { }},
                  };
                c.AddSecurityRequirement(securityRequirements);
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                c.TagActionsBy(api => api.GroupName);


                c.EnableAnnotations();
            });

            services.AddDbContext<MasterContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("masterContext")), 
ServiceLifetime.Transient);
            services.AddDbContext<MasterServiceContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("MasterApiServiceData")),
ServiceLifetime.Transient);


            services.AddOptions();
            services.Configure<TokenOptions>(Configuration.GetSection(nameof(TokenOptions)));
            services.AddHttpContextAccessor();

            services.Configure<IISOptions>(options => {

            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          );


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v3.1/swagger.json", "MasterClient API v3.1");
               // c.InjectStylesheet("../swagger-ui/netcare.css");
                c.RoutePrefix = "docs";
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseCors("CorsApi");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartSchool.API.Data;
using SmartSchool.API.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.API
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
            // services.AddDbContext<SmartContext>(
            //     context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            // );
            services.AddDbContext<SmartContext>(
                context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            );
            


            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);



            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var apiProviderDescription = services.BuildServiceProvider()
                .GetService<IApiVersionDescriptionProvider>();



            services.AddSwaggerGen(
                options =>
                {
                    foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(
                            description.GroupName //$"ssapi{}" //relacionado a URL, por isso aqui tudo junto
                            , new Microsoft.OpenApi.Models.OpenApiInfo()
                            {
                                Title = "SmartSchool API",
                                Version = description.ApiVersion.ToString(),//"1.0",
                                TermsOfService = new Uri("https://SeusTermosDeUso.com"),
                                Description = "Descri��o da API para SmartSchool",
                                License = new Microsoft.OpenApi.Models.OpenApiLicense
                                {
                                    Name = "Smart School License",
                                    Url = new Uri("http://mit.com")
                                },
                                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                                {
                                    Name = "Jo�o Baracat",
                                    Email = "",
                                    Url = new Uri("http://meusite.com")
                                }
                            });                                               
                    }

                    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    options.IncludeXmlComments(xmlCommentsFullPath);



                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Input the JWT like: Bearer {your token}",
                        Name = "Authorization",
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });


            

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiProviderDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"swagger/{description.GroupName}/swagger.json", 
                            description.GroupName.ToUpperInvariant()
                        );
                    }
                    options.RoutePrefix = "";
                });

            // app.UseAuthorization();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

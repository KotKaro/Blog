using System;
using System.IO;
using System.Reflection;
using Autofac;
using Blog.API.Infrastructure.AutofacModules;
using Blog.DataAccess;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Blog.API
{
    public class Startup
    {
        private const string CorsPolicyName = "MasAPpCorsPolicy";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddResponseCompression()
                .AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                })).AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog API", Version = "v1" });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }).AddMvc(options => { options.EnableEndpointRouting = false; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BlogDbContext blogDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                        c.RoutePrefix = string.Empty;
                    });
            }

            app.UseResponseCompression()
                .UseHttpsRedirection()
                .UseCors(CorsPolicyName)
                .UseRouting()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseMvc()
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "StaticFiles")),
                    RequestPath = "/.well-known/pki-validation"
                });

            blogDbContext.ApplyMigrations();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new ApplicationModule(_configuration));
        }
    }
}
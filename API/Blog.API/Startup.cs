using System;
using System.IO;
using System.Reflection;
using Autofac;
using Blog.API.Infrastructure.AutofacModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Blog.API
{
    public class Startup
    {
        private const string CorsPolicyName = "MasAPpCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MAS API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyName);

            app.UseRouting();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseMvc();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new ApplicationModule(Configuration));
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RESTFullApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace RESTFullApi
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

            //指定数据库上下文使用内存数据库
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            //添加Swagger中间件
            services.AddSwaggerGen(c =>
            {
                //Api信息和说明
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My Api",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://github.com/IServiceStudio/RESTFullApi.git"),
                    Contact = new OpenApiContact
                    {
                        Name = "Feng Lv",
                        Email = "lv.feng@outlook.com",
                        Url = new Uri("https://github.com/IServiceStudio")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "My Api",
                        Url = new Uri("https://github.com/IServiceStudio")
                    }
                });
                //启用XML注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //启用Swagger中间件
            app.UseSwagger();

            //启用SwaggerUI中间件(启用静态文件中间件)
            app.UseSwaggerUI(c =>
            {
                //启用自定义样式
                //c.InjectStylesheet("/swagger-ui/custom.css");
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "My Api V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

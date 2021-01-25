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

            //ָ�����ݿ�������ʹ���ڴ����ݿ�
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            //���Swagger�м��
            services.AddSwaggerGen(c =>
            {
                //Api��Ϣ��˵��
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
                //����XMLע��
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

            //����Swagger�м��
            app.UseSwagger();

            //����SwaggerUI�м��(���þ�̬�ļ��м��)
            app.UseSwaggerUI(c =>
            {
                //�����Զ�����ʽ
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Blog.Core.AOP;
using Blog.Core.AuthHelper.OverWrite;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Repository;
using Blog.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Blog.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Blog.Core API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Blog.Core", Email = "z@zhangxiangnan.cn", Url = "https://www.zhangxiangnan.cn" }
                });

                //var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "Blog.Core.xml");//这个就是刚刚配置的xml文件名
                //c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                //var xmlModelPath = Path.Combine(basePath, "Blog.Core.Model.xml");//这个就是Model层的xml文件名

                //c.IncludeXmlComments(xmlModelPath);

                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.Core", new string[] { } } };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入{token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion

            #region authorization

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });

            #endregion

            BaseDBConfig.ConnectionString = Configuration.GetSection("AppSettings:SqlServerConnection").Value;

            #region autofac

            services.AddScoped<ICaching, MemoryCaching>();

            var builder = new ContainerBuilder();

            builder.RegisterType<BlogCacheAOP>();//可以直接替换其他拦截器！一定要把拦截器进行注册

            var assemblysServices = Assembly.Load("Blog.Core.Services");
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(BlogCacheAOP));//指定已扫描程序集中的类型注册为提供所有其实现的接口。


            var assemblysRepository = Assembly.Load("Blog.Core.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            //builder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>();

            builder.Populate(services);

            var ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);

            #endregion



        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                });



                #endregion
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<JwtTokenAuth>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

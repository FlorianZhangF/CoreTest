using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Core.Utility.Filters;
using Core.Utility.Middlewares;
using Core.Web.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });

        //    //加入Session支持
        //    services.AddSession();
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        //}

        /// <summary>
        /// IOC换成Autofac
        /// Autofac支持AOP
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //加入Session支持
            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //加入全局异常Filter
            services.AddMvc(option =>
            {
                //异常处理Filter
                option.Filters.Add(typeof(CustomExceptionFilterAttribute));
                //ResourceFilter
                option.Filters.Add(typeof(CustomResourceFIlterAttribute));
            });

            //指定使用ServiceFilter标记的特性
            services.AddScoped<CustomActionFilterAttribute>();

            //实例一个容器
            ContainerBuilder containerBuilder = new ContainerBuilder();
            //Autofac接管默认IOC容器的其他工作，包括实例化控制器
            containerBuilder.Populate(services);

            #region 直接注册服务
            ////注册AOP
            //containerBuilder.Register(context => new CustomAutofacAOP(context));
            ////注册服务
            //containerBuilder.RegisterModule<CustomAutofacModule>();
            #endregion

            #region 根据配置文件注册服务
            //Autofac也可以用配置文件注入
            IConfigurationBuilder config = new ConfigurationBuilder();
            //读取配置文件
            config.AddJsonFile("autofac.json");
            // Register the ConfigurationModule with Autofac. 
            IConfigurationRoot configBuild = config.Build();
            //读取配置文件里配置需要注册的服务
            var module = new ConfigurationModule(configBuild);
            //注册服务
            containerBuilder.RegisterModule(module);
            #endregion

            //可以获取到一个接口的所有实现
            //IEnumerable<IServiceTest> testServices = container.Resolve<IEnumerable<IServiceTest>>();

            //返回需要的provider
            IContainer container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory factory)
        {
            //使用Log4net
            ILogger<Startup> _ilogger = factory.CreateLogger<Startup>();
            _ilogger.LogError($"this is a log test");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //根据条件指定中间件，没有next()
            app.Map("/Test", builder =>
             {
                 builder.Use(async (context, next) =>
                 {
                     await context.Response.WriteAsync($"Test text");
                     //await next();
                 });
             });
            
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("Name");
            }, builder =>
            {
                builder.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync($"Name text");
                    //await next();
                });
            });

            //使用外部定义的中间件
            app.UseMiddleware<UseMiddleware>();

            //自定义中间件，带next会往下执行，不带就是终结器
            app.Use(next =>
            {
                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("Use1 text start");
                    await next.Invoke(context);
                    await context.Response.WriteAsync("Use1 text end");
                });
            });

            app.Use(next =>
            {
                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("Use2 text start");
                    await next.Invoke(context);
                    await context.Response.WriteAsync("Use2 text end");
                });
            });

            app.Use(next =>
            {
                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("Use3 text start");
                    //await next.Invoke(context);
                    await context.Response.WriteAsync("Use3 text end");
                });
            });

            //使用Session
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //读取配置文件 XPath语法
            Console.WriteLine(this.Configuration["Logging:LogLevel:Default"]);

        }
    }
}

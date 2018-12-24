using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using DAL.Reposity.PostgreSqlRepository;
using Models.Interfaces.Repository;
using Models.Services;

namespace Localization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        ///  This method gets called by the runtime.Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ///TODO Уберу, сделаю как у всех. Такой метод выделяет память на класс, который может быть и не будет использоваться.
            ///Как идея использовать класс, который видно во всех методах.
            //var connectionString = Configuration.GetConnectionString("MyWebApiConnection");

            // 
            //services.AddScoped<IFilesRepository>(provider => new FilesRepository(connectionString));
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IGlossaryRepository, GlossaryRepository>();
            services.AddScoped<GlossaryService>();

            ////Данный блок кода включает доступ к серверу с любого порта(нужен для тестирования с нескольких клиентов)///////
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });
            //////////////Данный блок заканчивается

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ///TODO
            //services.AddEntityFrameworkNpgsql().AddDbContext<PostgreSqlEFContext>(opt => opt.UseNpgsql(connectionString)); // тут подключаем нашу удаленную бд

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseCors("SiteCorsPolicy"); // это тоже для нескольких портов(см. выше)

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

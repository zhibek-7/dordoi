﻿using Microsoft.AspNetCore.Builder;
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
using Microsoft.AspNetCore.Diagnostics;
using Utilities.Logs;
using System.Net;
using Models.DatabaseEntities;
using Models.Migration;

namespace Localization
{
    public class Startup
    {

        private readonly ExceptionLog _exceptionLog = new ExceptionLog();

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
            ///Как идея использовать класс, который видно во всех методах.
            //var connectionString = Configuration.GetConnectionString("db_connection");
            var connectionString = Settings.GetStringDB();
            services.AddScoped<ISetttings>(provider => new Settings(connectionString));


            // TODO нужно будет переделать все классы под этот вариант
            services.AddScoped<IFilesRepository>(provider => new FilesRepository(connectionString));
            services.AddScoped<IGlossaryRepository>(provider => new GlossaryRepository(connectionString));
            services.AddScoped<IGlossariesRepository>(provider => new GlossariesRepository(connectionString));
            services.AddScoped<ITranslationSubstringRepository>(provider => new TranslationSubstringRepository(connectionString));
            services.AddScoped<ITranslationTroubleRepository>(provider => new TranslationTroubleRepository(connectionString));
            services.AddScoped<ILocaleRepository>(provider => new LocaleRepository(connectionString));

            services.AddScoped<FromExcel>();


            services.AddScoped<FilesService>();
            services.AddScoped<GlossaryService>();
            services.AddScoped<GlossariesService>();
            services.AddScoped<UserAction>();

            /*
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<FilesService>();
            services.AddScoped<IGlossaryRepository, GlossaryRepository>();
            services.AddScoped<GlossaryService>();
            services.AddScoped<IGlossariesRepository, GlossariesRepository>();
            services.AddScoped<GlossariesService>();
            services.AddScoped<ITranslationSubstringRepository, TranslationSubstringRepository>();
            */
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
            app.UseExceptionHandler(appBuilder
                => appBuilder.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var unhandledException = errorFeature?.Error;
                    this._exceptionLog.WriteLn("Localization web app unhandled exception.", unhandledException);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    using (var streamWriter = new System.IO.StreamWriter(context.Response.Body))
                    {
                        await streamWriter.WriteLineAsync(unhandledException?.Message ?? string.Empty);
                    }
                }));

            if (!env.IsDevelopment())
            {
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

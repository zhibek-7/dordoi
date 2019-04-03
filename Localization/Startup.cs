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
using Microsoft.AspNetCore.Diagnostics;
using Utilities.Logs;
using System.Net;
using Models.DatabaseEntities;
using Models.Migration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Localization.Authentication;
using System;
using Utilities;
using Utilities.Mail;
using Models.Models;

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
            services.AddSingleton<ISettings>(provider => new Settings(connectionString));
            services.AddSingleton<SettingsModel>(provider => new SettingsModelService().DeserializeFromDefaultLocation());


            // TODO нужно будет переделать все классы под этот вариант
            services.AddScoped<IFilesRepository>(provider => new FilesRepository(connectionString));
            services.AddScoped<IGlossaryRepository>(provider => new GlossaryRepository(connectionString));
            services.AddScoped<IGlossariesRepository>(provider => new GlossariesRepository(connectionString));
            services.AddScoped<ITranslationSubstringRepository>(provider => new TranslationSubstringRepository(connectionString));
            services.AddScoped<ITranslationTroubleRepository>(provider => new TranslationTroubleRepository(connectionString));
            services.AddScoped<ILocaleRepository>(provider => new LocaleRepository(connectionString));
            services.AddScoped<IUserActionRepository>(provider => new UserActionRepository(connectionString));
            services.AddScoped<IMail>(provider => new EMail());
            services.AddScoped<ITranslationMemoryRepository>(provider => new TranslationMemoryRepository(connectionString));
            services.AddScoped<IInvitationsRepository>(provider => new InvitationsRepository(connectionString));
            services.AddScoped<IUserRepository>(provider => new UserRepository(connectionString));
            services.AddScoped<IParticipantRepository>(provider => new ParticipantRepository(connectionString));
            services.AddScoped<IImagesRepository>(provider => new ImageRepository(connectionString));
            services.AddScoped<IProjectTranslationMemoryRepository>(provider => new ProjectTranslationMemoryRepository(connectionString));
            services.AddScoped<IFilesPackagesRepository>(provider => new FilesPackagesRepository(connectionString));
            services.AddScoped<ITranslationTopicRepository>(provider => new TranslationTopicRepository(connectionString));
            services.AddScoped<ITypeOfServiceRepository>(provider => new TypeOfServiceRepository(connectionString));

            services.AddScoped<FromExcel>();


            services.AddScoped<FilesService>();
            services.AddScoped<GlossaryService>();
            services.AddScoped<GlossariesService>();
            services.AddScoped<UserAction>();
            services.AddScoped<TranslationMemoryService>();
            services.AddScoped<InvitationsService>();

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


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    AuthenticationOptions opt = new AuthenticationOptions();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,

                        // строка, представляющая издателя
                        ValidIssuer = opt.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = opt.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,

                        // установка ключа безопасности
                        IssuerSigningKey = opt.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            services.AddSignalR(options => options.EnableDetailedErrors = false);
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


            app.UseAuthentication(); // система аутентификации
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<Hubs.DataImport.DataImportHub>("/dataimport/hub");
                routes.MapHub<Hubs.Files.FilesHub>("/files/hub");
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

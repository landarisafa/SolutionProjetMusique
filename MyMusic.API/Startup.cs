using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MyMusic.Data;
using MyMusic.Data.MongoDB.Repository;
using MyMusic.Data.MongoDB.Setting;
using MyMusic.Services.Services;
using MyMusicCore;
using MyMusicCore.Services;
using MysMusic.Core.Repositories;
using MysMusic.Core.Services;

namespace MyMusic.API
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

            //Configuration for SQL server
            //D�finition du dbContect en lui donnant une chaine de connection string de l'appsetting 
            // et en indiquant le dll corresspondant (assembly)
            services.AddDbContext<MyMusicDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("MyMusic.Data")));
            //dependency Injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Configuration MongoDB
            services.Configure<Settings>(
            options =>
            {
                options.ConnectionString = Configuration.GetValue<string>("MongoDB:ConnectionString");
                options.Database = Configuration.GetValue<string>("MongoDB:Database");
            });
            //Singleton parceque tt l'application va utiliser une seule instance de connexion � la bd mongo
            services.AddSingleton<IMongoClient, MongoClient>(
          _ => new MongoClient(Configuration.GetValue<string>("MongoDB:ConnectionString")));

            //DI IDatabaseSettings
            services.AddTransient<IDatabaseSettings, DatabaseSettings>();

            //DI IComposerRepository
            services.AddScoped<IComposerRepository, ComposerRepository>();

            // Services 
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IComposerService, ComposerService>();

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "Put title here", Description = "DotNet Core Api 3 - with swagger" });
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Music V1");
            });
        }
    }
}

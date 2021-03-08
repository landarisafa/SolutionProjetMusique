using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MyMusic.Data;
using MyMusic.Data.MongoDB.Repository;
using MyMusic.Data.MongoDB.Setting;
using MyMusicCore;
using MysMusic.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            
            //
            services.AddScoped<IComposerRepository, ComposerRepository>();
            services.AddTransient<IDatabaseSettings, DatabaseSettings>();
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
        }
    }
}

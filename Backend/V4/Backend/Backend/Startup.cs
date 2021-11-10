using AutoMapper;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Backend
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
            services.AddDbContext<SetContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("Default"))
            );
            services.AddDatabaseDeveloperPageExceptionFilter();

            //Repositories  
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            
            
            //Services
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ISetService, SetService>();
            
            services.AddSingleton<ISeedService, NormalSeedService>();
            // services.AddSingleton<ISeedService, GenerateYamlSeedService>();
            
            //configure automapping
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddCors();
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Backend", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SetContext db, 
            ISeedService seedService, IGameService gameService, IPlayerRepository playerRepository)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
            seedService.Seed(db, gameService, playerRepository);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            }
            else
            {
                app.UseExceptionHandler(builder => builder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var error = new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        };

                        var jsonError = JsonSerializer.Serialize(error);
                        await context.Response.WriteAsync(jsonError);
                    }
                }));
            }

            app.UseRouting();
            
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
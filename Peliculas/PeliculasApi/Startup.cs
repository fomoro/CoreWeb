using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PeliculasApi.Contexts;
using PeliculasApi.Extensions;
using PeliculasApi.Mapper;
using PeliculasApi.Repository;
using PeliculasApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace PeliculasApi
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
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ApplicationDbContext>(Options => { Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); });

            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPeliculaRepository, PeliculaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            /*Agregar dependencia del token*/
            //services.AddAuthentication esta en services.ConfigureJwt
            services.ConfigureJwt(Configuration);

            services.AddAutoMapper(typeof(PeliculasMappers));

            services.AddControllers();          

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Titulo del proyecto", Version = "v1" });

                c.SwaggerDoc("ApiCategorias", new OpenApiInfo()
                {
                    Title = "API Categorías Películas",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new OpenApiContact()
                    {
                        Email = "fomoro@gmail.com.com",
                        Name = "fomoro",
                        Url = new Uri("https://fomoro.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                c.SwaggerDoc("ApiPeliculas", new OpenApiInfo()
                {
                    Title = "API Películas",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "admin@render2web.com",
                        Name = "render2web",
                        Url = new Uri("https://render2web.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                c.SwaggerDoc("ApiUsuarios", new OpenApiInfo()
                {
                    Title = "API Usuarios Películas",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "admin@render2web.com",
                        Name = "render2web",
                        Url = new Uri("https://render2web.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });


                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                c.IncludeXmlComments(rutaApiComentarios);

                //Primero definir el esquema de seguridad
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autenticación JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                  });
            });

            /*Damos soporte para CORS*/
            //services.ConfigureCors();
            services.AddCors();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto Base Eliminar v1"));
                app.UseSwaggerUI(c =>
                {    
                    
                    c.SwaggerEndpoint("/swagger/ApiCategorias/swagger.json", "API Categorías Películas");
                    c.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "API Películas");
                    c.SwaggerEndpoint("/swagger/ApiUsuarios/swagger.json", "API Usuarios Películas");
                    
                    //Para la publicación en IIS descomentar estas líneas y comentar las de arriba
                    //c.SwaggerEndpoint("/Back/swagger/ApiPeliculasCategorias/swagger.json", "API Categorías Películas");
                    //c.SwaggerEndpoint("/Back/swagger/ApiPeliculas/swagger.json", "API Películas");
                    //c.SwaggerEndpoint("/Back/swagger/ApiPeliculasUsuarios/swagger.json", "API Usuarios Películas");                    
                }); 
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseHttpsRedirection();           

            app.UseRouting();

            /*Estos dos son para la autenticación y autorización*/
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*Damos soporte para CORS*/
            //app.UseCors("CorsPolicy");
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}

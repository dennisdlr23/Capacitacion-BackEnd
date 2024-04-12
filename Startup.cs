using OrderPurchase.WebApi.Features.Auth;
using OrderPurchase.WebApi.Features.Common;
using OrderPurchase.WebApi.Features.Users;
using OrderPurchase.WebApi.Features.Users.Services;
using OrderPurchase.WebApi.Helpers;
using OrderPurchase.WebApi.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderPurchase.WebApi.Features.ServiceLayer;
using OrderPurchase.WebApi.Features.TypeDocuments.Services;
using OrderPurchase.WebApi.Features.DataMaster.Services;
using OrderPurchase.WebApi.Features.Orders.Service;

namespace OrderPurchase.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de Swagger para documentación de la API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderPurchase.WebApi", Version = "v1" });
            });

            // Configuración del contexto de base de datos
            services.AddDbContext<OrderPurchaseDbContext>(
                dbContextOptions => dbContextOptions
                    .UseSqlServer(Configuration.GetConnectionString("dbOrderPurchase"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

            // Registro de servicios mediante inyección de dependencias
            services.AddScoped<HanaDbContext>();

            services.AddTransient<AuthService, AuthService>();
            services.AddTransient<UserService, UserService>();
            services.AddTransient<CommonService, CommonService>();
            services.AddTransient<RoleService, RoleService>();
            services.AddTransient<PermissionService, PermissionService>();
            services.AddTransient<AuthSapServices, AuthSapServices>(); 
            services.AddTransient<OrderPurchaseServices, OrderPurchaseServices>();
            services.AddTransient<TypeDocumentServices, TypeDocumentServices>();
            services.AddTransient<DataMasterServices, DataMasterServices>();
            services.AddTransient<OrderServices, OrderServices>();

            // Configuración de autenticación mediante token
            services.AddTokenAuthentication(Configuration);

            // Agrega controladores MVC a los servicios
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderPurchaseApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Habilitamos los cors para usar en la web OJO Solo en pruebas en produccion hay que especificiar el origen
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderPurchase.WebApi v1"));
            app.UseAuthentication();
            app.UseAuthorization();
       

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

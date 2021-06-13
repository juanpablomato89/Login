using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Data;
using Microsoft.AspNetCore.Identity;
using Login.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Login.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Login
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
            services.AddDbContext<LoginContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LoginContext")));

            services.AddMvc(option =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    option.Filters.Add(new AuthorizeFilter(policy));
                }).AddXmlDataContractSerializerFormatters();



            services.AddIdentity<Usuario, IdentityRole>(options=> { })
                .AddEntityFrameworkStores<LoginContext>().AddErrorDescriber<IdentityErrorDescriber>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequiredUniqueChars = 3;
                opciones.Password.RequiredLength = 3;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireNonAlphanumeric = false;

            });

            services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Cuentas/Login";
                    options.AccessDeniedPath = "/Cuentas/AccesoDenegado";
                });

            services.AddAuthorization(options=>
                {
                   
                    options.AddPolicy("EditarRolPolice", policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole("Administrador") && context.User.HasClaim(claim => claim.Type == "Editar Rol")

                         )) ;
                    options.AddPolicy("BorrarRolPolice", policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole("Administrador") && context.User.HasClaim(claim => claim.Type == "Borrar Rol")

                         ));
                    options.AddPolicy("CrearRolPolice", policy => policy.RequireAssertion(
                        context =>
                        context.User.IsInRole("Administrador") && context.User.HasClaim(claim => claim.Type == "Crear Rol")

                         ));

                    options.AddPolicy("AdministradorPolice", policy => policy.RequireAssertion(
                       context =>
                       context.User.IsInRole("Administrador")
                       ));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}

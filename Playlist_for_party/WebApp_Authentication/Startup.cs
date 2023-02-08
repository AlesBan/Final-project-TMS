using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WebApp_Authentication.Policies;
using WebApp_Data.Interfaces;
using WebApp_Data.Models.Data;

namespace WebApp_Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddSingleton<IMusicRepository, MusicRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JWTSettings:Issuer"],
                        ValidAudience = Configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration["JWTSettings:SecretKey"])),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true
                    };
                });
            services.AddAuthorization(op => { op.AddPolicy(NamePolicy.Name, NamePolicy.Requirements); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "{controller=User}/{action=Home}");
                endpoints.MapControllerRoute(
                    name: "search/{query?}",
                    pattern: "{controller=User}/{action=Search}");
                endpoints.MapControllerRoute(
                    name: "playlist/{id?}",
                    pattern: "{controller=User}/{action=Playlist}");
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapControllerRoute(
                    name: "registration",
                    pattern: "{controller=Account}/{action=Registration}");
            });
        }
    }
}
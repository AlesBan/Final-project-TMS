using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using WebApp_Authentication.Policies;
using WebApp_Data.Interfaces;
using WebApp_Data.Models.Data;

namespace Playlist_for_party
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            services.AddHttpClient<IMusicService, SpotifyService>(c =>
            {
                c.BaseAddress = new Uri("https://api.spotify.com/v1/");
                c.DefaultRequestHeaders.Add("Accept", "application/.json");
            });

            var connection = Configuration.GetConnectionString("DefaultConnection");


            services.AddControllersWithViews();
            services.AddDbContext<MusicContext>(options => options.UseSqlServer(connection));
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

            app.UseAuthentication();
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
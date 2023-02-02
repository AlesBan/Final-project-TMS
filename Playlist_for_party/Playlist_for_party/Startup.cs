using System;
using System.Collections.Generic;
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
using Playlist_for_party.Configuration;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models;
using Playlist_for_party.Policies;
using Playlist_for_party.Services;

namespace Playlist_for_party
{
    public class Startup
    {
        public static IMusicRepository MusicRepository { get; set; } = new MusicRepository()
        {
            Users = new List<User>()
            {
                new User()
                {
                    UserName = "admin",
                    Password = "admin",
                    Roles = new List<string>() { "user", "admin" }
                },
                new User()
                {
                    UserName = "ales",
                    Password = "admin",
                    Roles = new List<string>() { "user", "admin" }
                }
            }
        };

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

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            services.AddControllersWithViews();
            services.AddDbContext<MusicContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<IMusicRepository, MusicRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config =>
                {
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey("token"))
                            {
                                context.Token = context.Request.Query["token"];
                            }

                            return Task.CompletedTask;
                            ;
                        }
                    };
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JWTSettings:Issuer"],
                        ValidAudience = Configuration["JWTSettings:Audience"],
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration["JWTSettings:SecretKey"]))
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

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "{controller=Home}/{action=Home}",
                    defaults: new { controller = "Home" });
                endpoints.MapControllerRoute(
                    name: "search/{query?}",
                    pattern: "{controller=Home}/{action=Search}");
                endpoints.MapControllerRoute(
                    name: "playlist/{id?}",
                    pattern: "{controller=Home}/{action=Playlist}");
            });
        }
    }
}
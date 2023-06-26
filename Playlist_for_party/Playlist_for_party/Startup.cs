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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using Playlist_for_party.Middleware;
using Playlist_for_party.Services;
using Playlist_for_party.Policies;
using Playlist_for_party.Services.Managers.DataManagers;
using Playlist_for_party.Services.Managers.UserManagers;

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
            services.AddDbContext<MusicContext>(options =>
            {
                options.UseSqlServer(connection);
                // options.EnableSensitiveDataLogging(true);
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddScoped<IDataManager, DataManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddSingleton<IAuthManager, AuthManager>();
            services.AddScoped<IPlaylistDataManager, PlaylistDataManager>();

            services.AddSingleton<SetTokenMiddleware>();

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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Headers["Authorization"];
                            return Task.CompletedTask;
                        },
                    };
                });
            services.AddAuthorization(op => { op.AddPolicy(NamePolicy.Name, NamePolicy.Requirements); });
            services.AddSession();
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
            app.UseSession();

            app.UseMiddleware<SetTokenMiddleware>();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{Id?}");
            });
        }
    }
}
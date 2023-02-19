using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Playlist_for_party
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(typeof(Controller).Assembly.GetName().Version);
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureLogging(logging => logging.AddConsole()
                //     .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information))
                .ConfigureLogging(logging=>logging.AddDebug())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
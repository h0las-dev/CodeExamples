using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SQLite;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TgBotApp.Data;
using TgBotApp.Jobs;
using TgBotApp.Services.Data;
using TgBotApp.Services.Telegram;

namespace TgBotApp
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
            services.AddDbContext<DataContext>(x =>
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(x => x.UseMemoryStorage());

            services.AddControllers();

            // Telegram.Bot doesn't work at .net core 3.0 without this now!!!
            services.AddMvc().AddNewtonsoftJson();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IReminderExecutorJob, ReminderExecutorJob>();
            services.AddTransient<IBaseRepository, BaseRepository>();
            services.AddTransient<IUpdateService, UpdateService>();
            services.AddSingleton<ITelegramBotService, TelegramBotService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITelegramBotService telegramBotService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<IReminderExecutorJob>(
                reminderExecutor => reminderExecutor.ExecuteReminders(), Cron.Minutely);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            telegramBotService.SetWebhookAsync().Wait();
        }
    }
}

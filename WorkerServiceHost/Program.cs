using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Hangfire.Jobs;
using Application.Core.Interfaces;
using Application.Core.Services;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;


public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args) 
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build();

        await host.RunAsync();
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        services.AddScoped<ApiDataUpsertJob>();

        services.AddHttpClient();

        services.AddScoped<IApiEntryService, ApiEntryService>();
        services.AddScoped<ApiEntryRepository>();

        services.AddDbContext<ApiDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<Action<IServiceProvider>>(provider => sp =>
        {
            using (var scope = provider.CreateScope())
            {
                var job = scope.ServiceProvider.GetRequiredService<ApiDataUpsertJob>();
                RecurringJob.AddOrUpdate("api-data-upsert", () => job.Execute(), Cron.Hourly());
            }

        });

        services.AddHostedService<HangfireJobScheduler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHangfireDashboard();
        app.UseHangfireServer();
    }

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}

public class HangfireJobScheduler : IHostedService
{
    private readonly Action<IServiceProvider> _schedule;
    private readonly IServiceProvider _serviceProvider;

    public HangfireJobScheduler(Action<IServiceProvider> schedule, IServiceProvider serviceProvider)
    {
        _schedule = schedule;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _schedule(_serviceProvider);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
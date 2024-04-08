using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Domain.SeedWork;
using ThaiDuongScada.Host.Application.Commands;
using ThaiDuongScada.Host.Application.Services;
using ThaiDuongScada.Host.Application.Stores;
using ThaiDuongScada.Host.Application.Workers;
using ThaiDuongScada.Infrastructure.Communication;
using ThaiDuongScada.Infrastructure;
using ThaiDuongScada.Infrastructure.Repositories;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<UpdateShiftReportWorker>();
            cfg.RegisterServicesFromAssemblyContaining<ApplicationDbContext>();
            cfg.RegisterServicesFromAssemblyContaining<Entity>();
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ThaiDuongScada.Api"));
            options.EnableSensitiveDataLogging();
        });

        var config = builder.Configuration;
        services.Configure<MqttOptions>(config.GetSection("MqttOptions"));
        services.AddSingleton<ManagedMqttClient>();

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IDeviceMouldRepository, DeviceMouldRepository>();
        services.AddScoped<IShiftReportRepository, ShiftReportRepository>();
        services.AddScoped<IMachineStatusRepository, MachineStatusRepository>();
        services.AddSingleton<MetricMessagePublisher>();

        services.AddSingleton<InjectionTimeBuffers>();

        services.AddHostedService<UpdateShiftReportWorker>();
    })
    .Build();

await host.RunAsync();

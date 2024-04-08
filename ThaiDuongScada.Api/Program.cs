using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ThaiDuongScada.Domain.SeedWork;
using ThaiDuongScada.Infrastructure;
using System.Reflection;
using ThaiDuongScada.Api.Application.Mapping;
using ThaiDuongScada.Api.Application.Hubs;
using ThaiDuongScada.Api.Application.Workers;
using ThaiDuongScada.Infrastructure.Communication;
using ThaiDuongScada.Infrastructure.Repositories;
using Buffer = ThaiDuongScada.Api.Application.Workers.Buffer;
using ThaiDuongScada.Api.Application.Queries.ShiftReports;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins("localhost",
                         "https://td-monitor.vercel.app",
                         "https://web-td-sub.vercel.app",
                         "https://web-td-monitor-dashboard.vercel.app",
                         "http://localhost:3000",
                         "http://localhost:5173",
                         "https://web-td-mes.vercel.app",
                         "https://web-cha-tags-manager.vercel.app",
                         "https://web-td-monitor-dashboard-demo.vercel.app",
                         "https://web-thaiduong-mes.vercel.app")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetValue("Authority", "");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddSignalR();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ThaiDuongScada.Api"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddAutoMapper(typeof(ModelToViewModelProfile));
builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblyContaining<ModelToViewModelProfile>();
        cfg.RegisterServicesFromAssemblyContaining<ApplicationDbContext>();
        cfg.RegisterServicesFromAssemblyContaining<Entity>();
    });

var config = builder.Configuration;
builder.Services.Configure<MqttOptions>(config.GetSection("MqttOptions"));
builder.Services.AddSingleton<ManagedMqttClient>();
builder.Services.AddSingleton<Buffer>();

builder.Services.AddHostedService<ScadaHost>();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceMouldRepository, DeviceMouldRepository>();
builder.Services.AddScoped<IShiftReportRepository, ShiftReportRepository>();
builder.Services.AddScoped<IMachineStatusRepository, MachineStatusRepository>();

builder.Services.AddScoped<IShiftReportService, ShiftReportService>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }
));

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();

using Hangfire;
using Hangfire.MemoryStorage;
using Serilog;
using Serilog.Events;
using SimpD;
using SimpD.Docker;
using SimpD.Dto;
using SimpD.Service;
using SimpD.Startup;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(
    c =>
        c.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()
);

builder.Services.AddHangfireServer();

builder.Services.AddAutoMapper(typeof(DtoProfile));
builder.Services.AddScoped<ContainerManager>();
builder.Services.AddScoped<DockerAdapter>();
builder.Services.AddScoped<DockerStatusProvider>();
builder.Services.AddScoped<GuiProvisioner>();
builder.Services.AddDbContext<MainContext>();
builder.Services.AddTransient<IStartupFilter, RunMigrations>();
builder.Services.AddTransient<IStartupFilter, ProvisionGui>();

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseHangfireDashboard();
app.MapControllers();
app.UseCors();

app.Run();

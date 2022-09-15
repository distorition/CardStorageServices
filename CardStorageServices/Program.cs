using CardStorageServices.Data;
using CardStorageServices.Models;
using CardStorageServices.Services.Impl;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DataBaseOptions>(options =>
{
    builder.Configuration.GetSection("Setting:DataBaseOPtions").Bind(options);
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields =HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.ResponseHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("X-Real-IP");
    logging.ResponseHeaders.Add("X-Forwad_For");
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
}).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });


builder.Services.AddDbContext<CardStorageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Setting:DataBaseOPtions:ConnectionString"]);
});


builder.Services.AddScoped<IClientPerositoryServices, ClientRepository>();
builder.Services.AddScoped<ICardRepositoryServices, CardRepository>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpLogging();

app.MapControllers();

app.Run();

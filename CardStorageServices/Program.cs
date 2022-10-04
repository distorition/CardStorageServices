using AutoMapper;
using CardStorageServices.Data;
using CardStorageServices.Mapping;
using CardStorageServices.Models;
using CardStorageServices.Models.Request.AuthenticationRequestResponse;
using CardStorageServices.Models.Request.CardRequestResponse;
using CardStorageServices.Models.Validators;
using CardStorageServices.Services;
using CardStorageServices.Services.Impl;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Настройки gRPC(подключение grpc сервиса)

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5001, lisenOptions =>
    {
        lisenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;//тут мы указываем какой протокол для передачи данных мы будем использовать
    });
    //таким образом мы каызваем что наш сервис будет слушать все сообещния с любого ИП адреса, так же указываем порт на котором будет слушание сообщений
});
builder.Services.AddGrpc();

#endregion

#region настройки Валидатора
builder.Services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestsValidator>();
builder.Services.AddScoped<IValidator<CreateCardRequest>, CardRequestValidation>();
#endregion

#region Настройки Маппера

var mapperProfile = new MapperConfiguration(mp => mp.AddProfile(new MappingsProfile()));
var mapper=mapperProfile.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

#region Настройки сервисов
builder.Services.Configure<DataBaseOptions>(options =>
{
    builder.Configuration.GetSection("Setting:DataBaseOPtions").Bind(options);
});
#endregion

#region Настройка Логина(авторизации) и логов
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
#endregion

#region Настройки JWT Токена

builder.Services.AddAuthentication(x =>//основные настройки аутентификации 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;// это значитт что мы будем работаь по предьявителю
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>// тут идет настройка самой схемы по который мы работаем 
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationServices.SecretKey)),//теперь система будет сама сверяться все токены которые будут к нам приходить с нашими клёч и сама будет узнать где наш токен или не наш(при помощи нашего секретного ключа котоырй мы ей передали)
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});


#endregion

#region Подключения базы данных
builder.Services.AddDbContext<CardStorageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Setting:DataBaseOPtions:ConnectionString"]);
});
#endregion

builder.Services.AddSingleton<IAuthenticateServices, AuthenticationServices>();

builder.Services.AddScoped<IClientPerositoryServices, ClientRepository>();
builder.Services.AddScoped<ICardRepositoryServices, CardRepository>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title="Стартовй Сервис",Version="v1"});//изменили заголовок

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()//добавялем защищенную секцию в которой будем хранить наш токен
    {
        Description ="Jwt Authorization header using the Beare",
        Name ="Authorization",
        In=ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()//таким образом мы указываем что вызов каждого метода требует вызова токена 
    {
        {//эти скобки нужня для доабвления чего либо в слвоарь то есть сначал ключ а потом парметры  

        new OpenApiSecurityScheme()//наш ключ 
        {
            Reference= new OpenApiReference()//а тут мы указываем что сам токена нужно брать из AddSecurityDefinition
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        Array.Empty<string>()//наши параметры 
        }
    });

});// при помощи этого метода мы можем менять наш фронт енд

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseWhen(c => c.Request.ContentType != "application/grpc",
    builder =>
    {
        builder.UseHttpLogging();
    });


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpLogging();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ClientService>();//нужна для того чтобы наше приложение могло обрабатывать сообщения grpс
    endpoints.MapGrpcService<CardService>();//так же указываем какой у нас тип сервиса(CardService)
});

app.MapControllers();

app.Run();

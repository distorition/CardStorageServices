using CardStorageServices.Data;
using CardStorageServices.Models;
using CardStorageServices.Services;
using CardStorageServices.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;

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

builder.Services.AddDbContext<CardStorageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Setting:DataBaseOPtions:ConnectionString"]);
});

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
        {
        new OpenApiSecurityScheme()
        {
            Reference= new OpenApiReference()//а тут мы указываем что сам токена нужно брать из AddSecurityDefinition
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        Array.Empty<string>()
        }
    });

});// при помощи этого метода мы можем менять наш фронт енд

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpLogging();

app.MapControllers();

app.Run();

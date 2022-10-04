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

#region ��������� gRPC(����������� grpc �������)

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5001, lisenOptions =>
    {
        lisenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;//��� �� ��������� ����� �������� ��� �������� ������ �� ����� ������������
    });
    //����� ������� �� �������� ��� ��� ������ ����� ������� ��� ��������� � ������ �� ������, ��� �� ��������� ���� �� ������� ����� �������� ���������
});
builder.Services.AddGrpc();

#endregion

#region ��������� ����������
builder.Services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestsValidator>();
builder.Services.AddScoped<IValidator<CreateCardRequest>, CardRequestValidation>();
#endregion

#region ��������� �������

var mapperProfile = new MapperConfiguration(mp => mp.AddProfile(new MappingsProfile()));
var mapper=mapperProfile.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion

#region ��������� ��������
builder.Services.Configure<DataBaseOptions>(options =>
{
    builder.Configuration.GetSection("Setting:DataBaseOPtions").Bind(options);
});
#endregion

#region ��������� ������(�����������) � �����
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

#region ��������� JWT ������

builder.Services.AddAuthentication(x =>//�������� ��������� �������������� 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;// ��� ������� ��� �� ����� ������� �� ������������
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>// ��� ���� ��������� ����� ����� �� ������� �� �������� 
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationServices.SecretKey)),//������ ������� ����� ���� ��������� ��� ������ ������� ����� � ��� ��������� � ������ ��� � ���� ����� ������ ��� ��� ����� ��� �� ���(��� ������ ������ ���������� ����� ������� �� �� ��������)
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});


#endregion

#region ����������� ���� ������
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
    c.SwaggerDoc("v1", new OpenApiInfo {Title="�������� ������",Version="v1"});//�������� ���������

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()//��������� ���������� ������ � ������� ����� ������� ��� �����
    {
        Description ="Jwt Authorization header using the Beare",
        Name ="Authorization",
        In=ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()//����� ������� �� ��������� ��� ����� ������� ������ ������� ������ ������ 
    {
        {//��� ������ ����� ��� ���������� ���� ���� � ������� �� ���� ������ ���� � ����� ��������  

        new OpenApiSecurityScheme()//��� ���� 
        {
            Reference= new OpenApiReference()//� ��� �� ��������� ��� ��� ������ ����� ����� �� AddSecurityDefinition
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        Array.Empty<string>()//���� ��������� 
        }
    });

});// ��� ������ ����� ������ �� ����� ������ ��� ����� ���

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
    endpoints.MapGrpcService<ClientService>();//����� ��� ���� ����� ���� ���������� ����� ������������ ��������� grp�
    endpoints.MapGrpcService<CardService>();//��� �� ��������� ����� � ��� ��� �������(CardService)
});

app.MapControllers();

app.Run();

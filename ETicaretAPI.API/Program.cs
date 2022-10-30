using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastucture;
using ETicaretAPI.Infrastucture.Filters;
using ETicaretAPI.Infrastucture.Services.Storage.Azure;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using System.Text;
using static Serilog.Sinks.MSSqlServer.ColumnOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistencesServices();
builder.Services.AddInfrastuctureService();
builder.Services.AddApplicationService();

//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ETicaretAPI.Infrastucture.Enums.StorageType.Local);


//var colOptions = new ColumnOptions();
//colOptions.Store.Add(StandardColumn.LogEvent);
////colOptions.Id.DataType = System.Data.SqlDbType.BigInt;
////colOptions.Message.DataType = SqlDbType.VarChar;
////colOptions.MessageTemplate.DataType = SqlDbType.VarChar;
////colOptions.Level.DataType = SqlDbType.NVarChar;
////colOptions.TimeStamp.DataType = SqlDbType.DateTime2;
////colOptions.Exception.DataType = SqlDbType.VarChar;
////colOptions.Properties.DataType = SqlDbType.Xml;
////colOptions.LogEvent.DataType = SqlDbType.NVarChar;
//colOptions.AdditionalColumns.Add(new()
//{
//    ColumnName = "UserName",
//    PropertyName = "UserName",
//    DataType = SqlDbType.VarChar,
//    NonClusteredIndex = true,
//    DataLength = 64
//});

var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn
            {ColumnName = "UserName", PropertyName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 164, AllowNull = true},

        new SqlColumn
            {ColumnName = "UserId", DataType = SqlDbType.BigInt, NonClusteredIndex = true, AllowNull = true},

        new SqlColumn
            {ColumnName = "RequestUri", DataType = SqlDbType.NVarChar, DataLength = -1, AllowNull = true},
    }
};


Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
            builder.Configuration.GetConnectionString("SqlServer"),
            "logs",
            autoCreateSqlTable: true,
            columnOptions: columnOptions
            )
    .WriteTo.Seq(builder.Configuration["Urls:Seq"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(log =>
{
    log.LoggingFields = HttpLoggingFields.All;
    log.RequestHeaders.Add("sec-ch-ua");
    log.ResponseHeaders.Add("MyResponseHeader");
    log.MediaTypeOptions.AddText("application/javascript");
    log.RequestBodyLogLimit = 4096;
    log.ResponseBodyLogLimit = 4096;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanýcý belirlediðimiz deðerdir. -> www.bilmemne.com
            ValidateIssuer = true, //Oluþturulacak token deðerini kimin daðýttýný ifade edeceðimiz alandýr. -> www.myapi.com
            ValidateLifetime = true, //Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
            ValidateIssuerSigningKey = true, //Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden suciry key verisinin doðrulanmasýdýr.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddControllers(opt=> opt.Filters.Add<ValidationFilter>())
    .AddFluentValidation(conf=> conf.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(opt=> opt.SuppressModelStateInvalidFilter = true);
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseHttpLogging();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next)=>
{
    context.Response.Headers["MyResponseHeader"] =
        new string[] { "My Response Header Value" };

    var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("UserName", userName);

    await next();
});

app.MapControllers();

app.Run();

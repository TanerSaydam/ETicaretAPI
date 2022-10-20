using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastucture;
using ETicaretAPI.Infrastucture.Filters;
using ETicaretAPI.Infrastucture.Services.Storage.Azure;
using ETicaretAPI.Infrastucture.Services.Storage.Local;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistencesServices();
builder.Services.AddInfrastuctureService();

builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ETicaretAPI.Infrastucture.Enums.StorageType.Local);

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

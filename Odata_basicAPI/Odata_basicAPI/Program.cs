using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Odata_basicAPI.DTOs;
using Odata_basicAPI.Model;
using Odata_basicAPI.Services;
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Skills>("Skills");
    builder.EntityType<Skills>().Filter().Count().Expand().OrderBy().Page();
    builder.EntityType<SkillsDTO>().HasKey(p => p.Id);
    builder.EntityType<SkillsDTO>().Filter().Count().Expand().OrderBy().Page();
    builder.EnableLowerCamelCase();
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);
//Add Mapper
builder.Services.AddSingleton<IMapper>(mapper => {
    var mappingConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new AutoMapperProfile());
    });
    return mappingConfig.CreateMapper();
});
//Add Mongodb Server
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DataBaseConnection");
    return new MongoClient(connectionString);
});

// Add services to the container.

builder.Services.AddControllers().AddOData(opt=> opt
.AddRouteComponents("odata",GetEdmModel())
.Select()
.Filter()
.OrderBy()
.SetMaxTop(20)
.Count()
.Expand()
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Cros Origin Policy to allow mongodb
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
//Json Serlializer Addition
builder.Services.AddControllersWithViews().AddNewtonsoftJson(
                a => a.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .AddNewtonsoftJson(
                b =>
                b.SerializerSettings.ContractResolver = new DefaultContractResolver()
                );
//Build Application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();

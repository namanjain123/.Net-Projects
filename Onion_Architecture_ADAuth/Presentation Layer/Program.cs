using Domain_Layer.Data;
using Domain_Layer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.Interfaces;
using Repository_Layer;
using Service_Layer;
using Service_Layer.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AzureAD_OAuth_API", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri("https://login.microsoftonline.com/60879ec9-9cda-42a6-9611-4bc2ee2fcedc/oauth2/v2.0/authorize"),
                TokenUrl = new Uri("https://login.microsoftonline.com/60879ec9-9cda-42a6-9611-4bc2ee2fcedc/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                            {
                                { "api://1642dc16-891c-4f3c-9769-6c7f38e2a989/ReadWriteAccess", "Reads the Weather forecast" }
                            }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                     {
                     new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                        },
                                Scheme = "oauth2",
                                Name = "oauth2",
                                In = ParameterLocation.Header
                     },
                        new List<string>()
                     }
                });
});
//Sql Dependency Injection
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddControllers();
//Addition of dependencies of Repository and Service Layer
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomer<Student>, StudentService>();
builder.Services.AddScoped<ICustomer<Department>, DepartemntService>();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
//JWT Wise
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "naman",
//        ValidAudience = "naman@example.com",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Namanjain9460202770an#"))
//    };
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureAD_OAuth_API v1");
        //c.RoutePrefix = string.Empty;
        c.OAuthClientId("1642dc16-891c-4f3c-9769-6c7f38e2a989");
        c.OAuthClientSecret("Mp5YKhbwbd4hUh3dDk9Ca.61RTd_Sv~IT~");
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    }
                );
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();

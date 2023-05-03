using FastEndpoints;

var build = WebApplication.CreateBuilder();
build.Services.AddFastEndpoints();
var app = build.Build();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();

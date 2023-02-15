using AsyncTypeAPI.Database;
using AsyncTypeAPI.Dtos;
using AsyncTypeAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Addition of the sql server connection
builder.Services.AddPooledDbContextFactory<reqDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Post EndPoint
app.MapPost("api/products", async (reqDbContext context, Request req) =>
{
    if (req == null)
    {
        return Results.BadRequest();
    }
    req.RequestBody = "Accepted";
    req.EstimatedTime = DateTime.Now.ToString();
    await context.Request.AddAsync(req);
    await context.SaveChangesAsync();
    return Results.Accepted($"api/products/{req.RequestId}",req);
}
);
//Status Endpoint
app.MapGet("api/product/{requestId}", (reqDbContext context, string requestId) =>
{
    var listReq = context.Request.FirstOrDefault(l => l.RequestBody == requestId);
    if (listReq == null)
    {
        return Results.NotFound();
    }

    RequestStatus status = new RequestStatus
    {
        RequestBody = listReq.RequestBody,
        ReosurceUrl = String.Empty
    };
    if (listReq.RequestBody!.ToUpper() == "COMPLETE")
    {
        status.ReosurceUrl = $"api/v1/products/{Guid.NewGuid().ToString()}";
        //return Results.Ok(listingStatus);

        return Results.Redirect("https://localhost:7076/" + status.ReosurceUrl);
    }

    status.EstimatedTime = DateTime.Now.ToString();
    return Results.Ok(status);
});
app.MapGet("api/product/{requestId}", (string requestId) =>
{
    return Results.Ok("This is where you would pass back the final result");
});
app.UseHttpsRedirection();
app.Run();



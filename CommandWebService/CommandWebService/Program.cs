using CommandWebService.AsyncDataServices;
using CommandWebService.Data;
using CommandWebService.EventProcessing;
using CommandWebService.SyncDataClient.Grpc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICommandRepo,CommandRepo>();
builder.Services.AddSingleton<IEventProcessor,EventProcessor>();
builder.Services.AddScoped<IPlatformDataClient,PlatformDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(opt=>{
    opt.UseInMemoryDatabase("InMem");
});

builder.Services.AddHostedService<MessageBusSubcriber>();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);
app.Run();

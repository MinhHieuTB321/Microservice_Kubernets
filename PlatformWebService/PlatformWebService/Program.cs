using Microsoft.EntityFrameworkCore;
using PlatformWebService.AsyncDataServices;
using PlatformWebService.Data;
using PlatformWebService.SyncDataServices.Grpc;
using PlatformWebService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var _env= builder.Environment;

if(_env.IsProduction()){
    Console.WriteLine("--> Using SqlServer Db:");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration["ConnectionStrings:PlatformsConn"]);
        });
}else{
    Console.WriteLine("--> Using In-mem Db:");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseInMemoryDatabase("InMem");
    });
}


builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


Console.WriteLine($"--> Command Service Endpoint {builder.Configuration["CommandService"]}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints=>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<GrpcPlatformServices>();

    endpoints.MapGet("/protos/platforms.proto",async context=>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
    });
});

PrepDb.PrepPopulation(app,_env.IsProduction());

app.Run();

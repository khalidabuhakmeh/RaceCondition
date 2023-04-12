using RaceCondition.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HelloWorld>();

var app = builder.Build();

app.UseMiddleware<HelloWorld>();

app.MapGet("/", () => "Hello World!");

app.Run();
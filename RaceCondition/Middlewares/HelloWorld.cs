namespace RaceCondition.Middlewares;

public class HelloWorld : Middleware
{
    private readonly ILogger<HelloWorld> _logger;

    public HelloWorld(IHostApplicationLifetime lifetime, ILogger<HelloWorld> logger) 
        : base(lifetime)
    {
        _logger = logger;
    }

    protected override Task InitializeAsync(CancellationToken cancellationToken)
    {
        // will fail here 💣
        _logger.LogInformation("Hello from Hello World!");
        return Task.CompletedTask;
    }

    protected override async Task ProcessAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Processing request");
        await next(context);
    }
}
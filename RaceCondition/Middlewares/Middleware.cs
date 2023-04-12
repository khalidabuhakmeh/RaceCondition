namespace RaceCondition.Middlewares;

public abstract class Middleware: IMiddleware, IDisposable
{
    private Task? _initializationTask;

    protected Middleware(IHostApplicationLifetime lifetime)
    {
        var startRegistration = default(CancellationTokenRegistration);
        lifetime.ApplicationStarted.Register(() =>
        {
            _initializationTask = InitializeAsync(lifetime.ApplicationStopping);
            startRegistration.Dispose();
        });
    }

    protected virtual Task InitializeAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Take a copy to avoid race conditions
        var task = _initializationTask;
        if (task != null)
        {
            // Wait until initialization is complete before passing the request to next middleware
            await task;

            // Clear the task so that we don't await it again later.
            _initializationTask = null;
        }
        
        await ProcessAsync(context, next);
    }

    protected abstract Task ProcessAsync(HttpContext context, RequestDelegate next);

    public void Dispose()
    {
        _initializationTask?.Dispose();
    }
}
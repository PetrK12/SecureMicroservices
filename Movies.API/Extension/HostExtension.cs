using Movies.API.Data;

namespace Movies.API.Extension;

public static class HostExtension
{
    public static async Task<IHost> SeedDatabase(this IHost host)
    {
        using var services = host.Services.CreateScope();
        var context = services.ServiceProvider.GetService<MoviesAPIContext>();
        if (context != null) await MoviesContextSeed.SeedAsync(context);
        return host;
    }
}
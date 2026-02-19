namespace DPK.Mosavabat.Api;

/// <summary>
/// Main program class
/// </summary>
public class Program
{
    /// <summary>
    /// Main method
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        CurrentDirectoryHelpers.SetCurrentDirectory();
        CreateWebHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Web host builder creator
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return new WebHostBuilder()
            .UseKestrel(opt =>
            {
                opt.AddServerHeader = true;
                opt.Limits.MaxRequestLineSize = 16 * 1024;
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging((_, logging) =>
            {
                logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            })
            .UseIIS()
            .UseIISIntegration()
            .UseStartup<Startup>();
    }

}
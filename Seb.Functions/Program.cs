using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Seb.Server.Server;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(
        (context, builder) =>
        {
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile(
                $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true);
            builder.AddEnvironmentVariables();
        })
    .ConfigureServices((context, services) =>
    {
        services.AddPersistance(context.Configuration["ConnectionStrings:DefaultConnection"]!)
            .AddMediatr()
            .AddClients(context.Configuration);
    })
    .Build();


host.Run();
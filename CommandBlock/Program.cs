using CommandBlock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();

var serviceCollection = new ServiceCollection();

serviceCollection.AddLogging(loggingBuilder => {
    var loggingSection = configuration.GetSection("Logging");
    loggingBuilder.AddFile(loggingSection, fileLoggerOptions =>
    {
        fileLoggerOptions.FormatLogEntry = (msg) =>
        {
            var stringBuilder = new System.Text.StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var jsonTextWriter = new JsonTextWriter(stringWriter);

            jsonTextWriter.WriteStartArray();
            jsonTextWriter.WriteValue(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            jsonTextWriter.WriteValue(msg.LogLevel.ToString());
            jsonTextWriter.WriteValue(msg.LogName);
            jsonTextWriter.WriteValue(msg.Message);
            jsonTextWriter.WriteValue(msg.Exception?.ToString());
            jsonTextWriter.WriteEndArray();

            return stringBuilder.ToString();
        };
    });
    loggingBuilder.AddConsole();
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

if (loggerFactory != null)
{
    var logger = loggerFactory.CreateLogger<Program>();

    logger.LogInformation("Starting...");

    var service = new Service(loggerFactory);

    var itemCommands = configuration.GetSection("ItemCommands").Get<List<ItemCommand>>();

    await service.Execute(itemCommands != null ? itemCommands : new List<ItemCommand>());

    logger.LogInformation("Finishing Successfully");
}
else
{
    throw new Exception("Error Create Logger");
}



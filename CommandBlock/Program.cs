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

var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

logger.LogInformation("Starting...");

logger.LogInformation("Finishing Successfully");


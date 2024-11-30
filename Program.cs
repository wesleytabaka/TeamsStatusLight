using Microsoft.Extensions.Configuration;

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");
IConfigurationRoot Configuration = configurationBuilder.Build();
Console.WriteLine(Configuration["GraphAPIBaseUrl"]);

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace work
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseWindowsService()
               .ConfigureServices((hostContext, services) =>
               {
                   IConfiguration configuration = hostContext.Configuration;
                   WorkerOptions dataAppSettings = configuration
                      .GetSection("SendGrid") // nome da TAG que esta os parametros do SendGrid no appsettings
                      .Get<WorkerOptions>();

                   services.AddScoped<ISendGridClient>(s => new SendGridClient(new SendGridClientOptions
                   {// chave de acesso ao sendgrid
                       ApiKey = dataAppSettings.KEY
                   }));
                   services.AddHostedService<Worker>();
               });

        public class WorkerOptions
        {
            public string KEY { get; set; }

        }



    }
}

using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using work.core.SendGrid;

namespace work
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private CancellationToken cancellationToken;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }


        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }


        public string DeleteMSG(string queueUrl,string idMsg, IAmazonSQS sqsCliente)
        {
            var deleterequeste = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = idMsg
            };

          return sqsCliente.DeleteMessageAsync(deleterequeste).Result.HttpStatusCode.ToString();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scopeAWS = _serviceScopeFactory.CreateScope();
            var AWSConfig = scopeAWS.ServiceProvider.GetRequiredService<IConfiguration>();

            // abre a conexao com  AWS passando as credenciais pelo appsettings.json
            IAmazonSQS sqs = new AmazonSQSClient(AWSConfig["AWS:awsAccessKeyId"], AWSConfig["AWS:awsSecretAccessKey"], RegionEndpoint.SAEast1);
           
            // faz a busca da url da fila pelo seu nome
            var queueUrl = sqs.GetQueueUrlAsync(AWSConfig["AWS:QueueUrl"]).Result.QueueUrl; 
            var receiveMessageRequest = new ReceiveMessageRequest
            {             
                MaxNumberOfMessages = 10,   // numero de MSG que vai buscar de uma vez                
                QueueUrl = queueUrl,       // url da fila               
                WaitTimeSeconds = 20,     // tempo que vai ficar aguardando uma MSG ficar disponivel

            };



            while (!stoppingToken.IsCancellationRequested)
            {      
                // sendgrid
                using var scope = _serviceScopeFactory.CreateScope();
                var sendGridClient = scope.ServiceProvider.GetRequiredService<ISendGridClient>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                // AWS
                var receiveMenssageResponse = sqs.ReceiveMessageAsync(receiveMessageRequest).Result;
                if (receiveMenssageResponse.Messages.Count > 0)
                {// se tiver mensagens para ler
                    foreach (var mensagens in receiveMenssageResponse.Messages)
                    {
                        try
                        {
                           // carrega o json da fila com os parametros:assunto, remetente, destinatario, conteudo
                           var DadosMSG = JsonConvert.DeserializeObject<email>(mensagens.Body);

                           // envio da mensagem 
                           if (SendEmails.Send(sendGridClient, DadosMSG))
                            {// mensage foi enviada com sucesso é eliminada da fila 
                                DeleteMSG(queueUrl, mensagens.ReceiptHandle, sqs);
                           }

                         }
                         catch (Exception ex)
                         {
                            throw (ex.InnerException);
                         }

                    }

                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Producer.business;
using System;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {

            var _awsAccessKeyId = "aqui_vai_a_AccessKeyId";
            var _awsSecretAccessKey = "aqui_vai_a_SecretAccessKey";
            var _nameQueue = "nome_da_fila_na_AWS";
            var _from = "Email_do_remetente@email.com";
            var _recipient = "Email_do_destinatario@email.com";
            var _subject = "assunto do email";
            var _content = "conteudo do email";

            var dadosEmail = new email(_subject, _recipient, _from, _content);


            string msg = JsonConvert.SerializeObject(dadosEmail);

            // acesso ao cliente SQS da AWS
            IAmazonSQS sqs = new AmazonSQSClient(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.SAEast1);


            // conexao com a fila pelo nome
            var sqsRequest = new CreateQueueRequest
            {
                QueueName = _nameQueue
            };


            var createQueueResponse = sqs.CreateQueueAsync(sqsRequest).Result;
            // get na url da fila pelo nome
            var myQueueUrl = createQueueResponse.QueueUrl;

            var listQueueRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueuesAsync(listQueueRequest);


            var sqsMessageRequest = new SendMessageRequest
            {
                QueueUrl = myQueueUrl,
                MessageBody = msg
            };



            if (sqs.SendMessageAsync(sqsMessageRequest).Result.HttpStatusCode.ToString() == "OK")
            {
                Console.WriteLine("menssagem enviada");
            }
            else
            {
                Console.WriteLine("nao menssagem enviada");
            }


        }
    }
    
}

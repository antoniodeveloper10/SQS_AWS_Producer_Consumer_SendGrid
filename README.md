# SQS_AWS_Producer_Consumer_SendGrid
Projeto contendo um Produtor de envio de emails para a SQS da AWS e um Consumidor em formato de JOB que faz as leituras periodicas da fila em buscas de novas mensagens e faz o envio de emails usando uma integração com o SendGrid e posteriormente ao envio com sucesso, este JOB apaga a mensagem da fila para que a mesma não volte a ser enviada


## Requisitos

 * [SendGrid](https://sendgrid.com/)
 * [SQS - AWS](https://docs.aws.amazon.com/pt_br/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-setting-up.html/) 
 * [Nuget AWSSDK.SQS - Ambos Projetos](https://www.nuget.org/packages/AWSSDK.SQS/)
 
 
### Criação
Depois de criada a conta e posteriormente a fila na AWS é preciso passar valores das credencais do usuario e o nome da fila para permitir o acesso ao cliente SQS 

### Producer
```
            var _awsAccessKeyId = "aqui_vai_a_AccessKeyId";
            var _awsSecretAccessKey = "aqui_vai_a_SecretAccessKey";
            var _nameQueue = "nome_da_fila_na_AWS";
            var _from = "Email_do_remetente@email.com";
            var _recipient = "Email_do_destinatario@email.com";
            var _subject = "assunto do email";
            var _content = "conteudo do email";
```

### Consumer 
No arquivo appsettings.json substituir o conteudo das variaveis
```
  "SendGrid": {
    "KEY": "aqui_vai_a_KEY_SendGrid"
  },

  "AWS": {
    "awsAccessKeyId": "aqui_vai_a_AccessKeyId", 
    "awsSecretAccessKey": "aqui_vai_SecretAccessKey", 
    "QueueUrl": "nome_da_fila_na_AWS"  
  }
}

```



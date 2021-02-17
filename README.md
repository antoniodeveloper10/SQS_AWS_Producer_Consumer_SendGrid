# SQS_AWS_Producer_Consumer_SendGrid
Projeto contendo um Produtor de envio de emails para a SQS da AWS  e um Consumidor em formato de JOB que faz as leituras da fila e faz o envio de emails usando uma integração com o SendGride 


## Requisitos

 * [SendGrid](https://sendgrid.com/)
 * [SQS - AWS](https://docs.aws.amazon.com/pt_br/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-setting-up.html/) 
 
 
### Criação
Depois de criada a conta e posteriormente a fila na AWS é preciso passar valores das credencais do usuario para acesso ao cliente SQS e o nome da fila criada.

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
no arquivo appsetings.json substituir o conteudo das variaveis
```
            "SendGrid": {
    "KEY": "aqui_vai_a_KEY_SendGrid"
  },

  "AWS": {
    "awsAccessKeyId": "aqui_vai_a_AccessKeyId", // 
    "awsSecretAccessKey": "aqui_vai_SecretAccessKey", 
    "QueueUrl": "nome_da_fila_na_AWS" // 
  }
}

```




No terminal da sua IDE execute estes comandos.

Em **nome_do_seu_cluster** substitua pelo nome que desejar:
```sh
kind create cluster --name=nome_do_seu_cluster
```

Suba o deployment da base 
```sh
kubectl apply -f base/deplyment.yaml
```

Suba o serviço da base 
```sh
kubectl apply -f base/service.yaml
```

**(opcional)** caso queira acessar a base pelo seu computador execute o **port-forward**
```sh
kubectl port-forward service/db  5432:5432
```

Suba o deployment da API 
```sh
kubectl apply -f api/deplyment.yaml
```

Suba o serviço da API 
```sh
kubectl apply -f api/service.yaml
```

Permitir acesso a API pelo seu computador execute o **port-forward**
```sh
kubectl port-forward service/api-service  8000:8000
```

Para acessar a API na barra do navegador 
```sh
digite http://127.0.0.1:8000/
```
Para acessar o painel de Admin da API
```sh
digite http://127.0.0.1:8000/admin
```
**E entre com os dados de usuário setados no deployment.yaml da api**

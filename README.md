# API - Fund Transfer
[![NPM](https://img.shields.io/npm/l/react)](https://github.com/grecojoao/fund-transfer/blob/master/LICENSE) 

## ⚡ Sobre o projeto

API Fund Transfer é uma aplicação back-end desenvolvida para realizar transações entre contas.

A aplicação possui o EndPoint: "/api/fund-transfer" do tipo Post, que é responsável por receber o PayLoad com as informações da transação:
````
{
  "accountOrigin": (string),
  "accountDestination": (string),
  "value": (int)
}
````
E responder com um identificador para a transação:
````
{
  "transactionId": (Guid)
}
````
Regras de négocio: 

- Conta de origem e destino devem ser informadas;
- Valor a ser transferido deve ser informado e maior que zero;



Também possui o Endpoint: "/api/fund-transfer/transactionId" do tipo Get, que recebe na rota da requisição o identificador da transação que se quer consultar.
Retornando as informações de status da transação:
````
{
  "transferStatus": (string),
  "message": (string)
}
````

Regras de négocio: 

- Identificador da transação deve ser informado;


## :rocket: Tecnologias
- C#, ASP.NET Core(6.0)
- Docker
- RabbitMq
- RavenDb

## 📝 Como executar o projeto
Pré-requisitos: 
- ASP.NET Core Runtime 6.0.1 ou 
- SDK 6.0.1(desenvolvimento)
- RabbitMq rodando em localhost na porta padrão ou
- RabbitMq rodando e configuração da aplicação no AppSettings apontando para o RabbitMq
- RavenDb rodando em localhost na porta padrão ou
- RavenDb rodando e configuração da aplicação no AppSettings apontando para o RavenDb

````bash
# clonar o repositório
git clone https://https://github.com/grecojoao/fund-transfer.git

# entrar na pasta da API
cd fund-transfer\src\Application\FundTransfer.Api

# restaurar as dependências
dotnet restore

# executar o projeto
dotnet watch run
````

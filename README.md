# Projeto de Arquitetura de Software 6SOAT

## quickfood-backend

Este projeto visa o desenvolvimento do backend para um software que simula um totem de autoatendimento de uma lanchonete.<br>
Utilizando a arquitetura limpa, .NET 8, Docker e Kubernetes, o objetivo é criar uma base sólida e escalável para suportar as funcionalidades necessárias para um sistema de autoatendimento. <br>
O foco principal é a criação de uma aplicação robusta, modular e de fácil manutenção.<br>

## Funcionalidades Principais

- **Gerenciamento de Pedidos**: Criação, atualização e consulta de pedidos feitos pelos clientes. <br>
- **Gerenciamento de Cardápio**: Manutenção dos itens do cardápio, incluindo adição, remoção e atualização de produtos. <br>
- **Gerenciamento de Cliente**: Manutenção dos cliente, incluindo adição, remoção e atualização de clientes. <br>
- **Pagamento dos Pedidos**: Integração (fake) com gateway de pagamentos. <br>
- **Armazenamento de Dados**: Persistência de dados utilizando um banco de dados adequado SQL Server. <br>

## Tecnologias Utilizadas

- **.NET 8**: Framework principal para desenvolvimento do backend. <br>
- **Arquitetura Limpa**: Estruturação do projeto para promover a separação de preocupações e facilitar a manutenção e escalabilidade. <br>
- **Docker**: Containerização da aplicação para garantir portabilidade e facilitar o deploy. <br>
- **Kubernetes**: Orquestração dos container visando resiliência da aplicação <br>
- **Banco de Dados**: Utilização do SQL Serverpara armazenamento de informações. <br>

## Estrutura do Projeto

A arquitetura limpa será utilizada para garantir que a aplicação seja modular e de fácil manutenção, o projeto foi estruturado da seguinte forma: API, Controllers, Gateways, Presenters, Domain, Infra (implementação das interfaces de repositório, configurações de banco de dados) e Building Blocks (componentes e serviços reutilizáveis)<br>

## Como Executar o Projeto

1. Clone este repositório:
```
git clone https://github.com/SofArc6Soat/quickfood-backend.git
```
2. Executar docker-compose ou kubernetes
   2.1. Docker (docker-compose)
      2.1.1. Navegue até o diretório do projeto:
```
cd quickfood-backend\src\DevOps
```
      2.1.2. Configure o ambiente Docker:
```
docker-compose up --build
```
      2.1.3. A aplicação estará disponível em http://localhost:5000 ou https://localhost:5001
      2.1.4. URL do Swagger: http://localhost:5000/swagger ou https://localhost:5001/swagger
      2.1.5. URL do Healthcheck da API: http://localhost:5000/health ou https://localhost:5001/health

   2.2 Kubernetes
      2.2.1 Navegue até o diretório do projeto:
```
cd quickfood-backend\src\DevOps\kubernetes
```
      2.2.2 Configure o ambiente Docker:
```
kubectl apply -f 01-sql-data-pvc.yaml
kubectl apply -f 02-sql-log-pvc.yaml
kubectl apply -f 03-sql-secrets-pvc.yaml
kubectl apply -f 04-quickfood-sqlserver-deployment.yaml
kubectl apply -f 05-quickfood-sqlserver-service.yaml
kubectl apply -f 06-quickfood-backend-deployment.yaml
kubectl apply -f 06-quickfood-backend-deployment.yaml
kubectl apply -f 07-quickfood-backend-service.yaml
kubectl apply -f 08-quickfood-backend-hpa.yaml
kubectl port-forward svc/quickfood-backend 8080:80
```
    ou executar todos scripts via PowerShell
```
Get-ExecutionPolicy
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
.\delete-k8s-resources.ps1
.\apply-k8s-resources.ps1
```
      2.2.3. A aplicação estará disponível em http://localhost:8080
      2.2.4. URL do Swagger: http://localhost:8080/swagger
      2.2.5. URL do Healthcheck da API: http://localhost:8080/health

## Collection com todas as APIs com exemplos de requisição
1. Caso deseje testar via postman com dados fake importe o arquivo "API QuickFood.postman_collection" do diretorio "src/DevOps/postman" na aplicação postman local.

2. Também é possível utilizar o Swagger disponibilizado pela aplicação (URL do Swagger: http://localhost:8080/swagger).

Para testar localmente com o Postman, a ordem de excução deverá ser a seguinte:
1. POST - {{baseUrl}}/pedidos (criar um pedido)
2. POST - {{baseUrl}}/pagamentos/checkout/:pedidoId (efetar checkout do pedido com integração fake para gerar o QRCODE do PIX)
3. POST - {{baseUrl}}/pagamentos/notificacoes/:pedidoId (simulação do WebHook que recebe que o PIX foi pago)
4. PATCH - {{baseUrl}}/pedidos/status/:pedidoId (altera status do pedido para "EmPreparacao" -> "Pronto" -> "Finalizado")

## Desenho da arquitetura
Para visualizar o desenho da arquitetura abra o arquivo "Arquitetura.drawio.png" no diretório "arquitetura" ou importe o arquivo "Arquitetura.drawio" no Draw.io (https://app.diagrams.net/).

## Demonstração em vídeo
Para visualizar a demonstração da aplicação acesse o seguinte link do Vimeo: https://vimeo.com/992479829/d8dba62df1

## Autores

- **Anderson Lopez de Andrade RM: 350452** <br>
- **Henrique Alonso Vicente RM: 354583**<br>

## Documentação Adicional

- **Miro - Domain Storytelling, Context Map, Linguagem Ubíqua e Event Storming**: [Link para o Event Storming](https://miro.com/app/board/uXjVKST91sw=/)
- **Github - Domain Storytelling**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-domain-story-telling)
- **Github - Context Map**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
- **Github - Linguagem Ubíqua**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)

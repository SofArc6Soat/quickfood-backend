# Projeto de Arquitetura de Software 6SOAT

## quickfood-backend

Este projeto visa o desenvolvimento do backend para um software que simula um totem de autoatendimento de uma lanchonete.<br>
Utilizando a arquitetura hexagonal, .NET 8 e Docker, o objetivo é criar uma base sólida e escalável para suportar as funcionalidades necessárias para um sistema de autoatendimento. <br>
O foco principal é a criação de uma aplicação robusta, modular e de fácil manutenção.<br>

## Funcionalidades Principais

- **Gerenciamento de Pedidos**: Criação, atualização e consulta de pedidos feitos pelos clientes. <br>
- **Gerenciamento de Cardápio**: Manutenção dos itens do cardápio, incluindo adição, remoção e atualização de produtos. <br>
- **Gerenciamento de Cliente**: Manutenção dos cliente, incluindo adição, remoção e atualização de clientes. <br>
- **Armazenamento de Dados**: Persistência de dados utilizando um banco de dados adequado SQL Server. <br>

## Tecnologias Utilizadas

- **.NET 8**: Framework principal para desenvolvimento do backend. <br>
- **Arquitetura Hexagonal**: Estruturação do projeto para promover a separação de preocupações e facilitar a manutenção e escalabilidade. <br>
- **Docker**: Containerização da aplicação para garantir portabilidade e facilitar o deploy. <br>
- **Banco de Dados**: Utilização do SQL Serverpara armazenamento de informações. <br>

## Estrutura do Projeto

A arquitetura hexagonal será utilizada para garantir que a aplicação seja modular e de fácil manutenção, a estrutura do projeto incluirá: <br>

- **API**: Endpoints para comunicação com clientes externos. <br>
- **Application**: Contém a lógica de aplicação, casos de uso e serviços de aplicação. <br>
- **BuildingBlocks**: Contém componentes e serviços reutilizáveis, como autenticação, autorização, log, manuseio de exceções, e outras funcionalidades transversais que podem ser compartilhadas entre diferentes partes da aplicação. <br>
- **Domain**: Contém as entidades de negócio, interfaces de repositório e serviços de domínio. <br>
- **Infra**: Contém a implementação das interfaces de repositório, configurações de banco de dados e integração com APIs externas. <br>

## Como Executar o Projeto

### 

1. Clone este repositório:
   ```bash
   git clone https://github.com/SofArc6Soat/quickfood-backend.git

2. Executar docker-compose ou kubernetes    

2.1 Docker (docker-compose)

2.1.1 Navegue até o diretório do projeto:
   ```bash
   cd quickfood-backend\src\DevOps
   
2.1.2 Configure o ambiente Docker:
   ```bash
   docker-compose up --build
   
2.2 Docker (kubernetes)

2.2.1 Navegue até o diretório do projeto:
   ```bash
   cd quickfood-backend\src\DevOps\kubernetes
   
2.2.2 Configure o ambiente Docker:
   ```bash
   kubectl apply -f 01-sql-data-pvc.yaml
   ```bash
   kubectl apply -f 02-sql-log-pvc.yaml
   ```bash
   kubectl apply -f 03-sql-secrets-pvc.yaml
   ```bash
   kubectl apply -f 04-quickfood-sqlserver-deployment.yaml
   ```bash
   kubectl apply -f 05-quickfood-sqlserver-service.yaml
   ```bash
   kubectl apply -f 06-quickfood-backend-deployment.yaml
   ```bash
   kubectl apply -f 06-quickfood-backend-deployment.yaml
   ```bash
   kubectl apply -f 07-quickfood-backend-service.yaml
   
ou executar todos scripts de uma vez   
   ```bash
   Get-ExecutionPolicy 
   ```bash
   Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
   ```bash
   .\apply-k8s-resources.ps1

3. A aplicação estará disponível em http://localhost:5000 ou https://localhost:5001

4. URL do Swagger: http://localhost:5000/swagger ou https://localhost:5001/swagger

5. URL do Healthcheck da API: http://localhost:5000/health ou https://localhost:5001/health

6. Caso deseje testar via postman com dados fake importe o arquivo API QuickFood.postman_collection.json do diretorio "postman" na aplicação postman local.


## Autores

- **Anderson Lopez de Andrade RM: 350452** <br>
- **Henrique Alonso Vicente RM: 354583**<br>

## Documentação Adicional

- **Miro - Domain Storytelling, Context Map, Linguagem Ubíqua e Event Storming**: [Link para o Event Storming](https://miro.com/app/board/uXjVKST91sw=/)
- **Github - Domain Storytelling**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-domain-story-telling)
- **Github - Context Map**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
- **Github - Linguagem Ubíqua**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
- **DockerHub - docker push sofarc6soat/quickfood-backend:tagname

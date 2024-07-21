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

1. Clone este repositório:
   ```bash
   git clone https://github.com/QuickFoodFiap/quickfood-backend.git

2. Navegue até o diretório do projeto:
   ```bash
   cd quickfood-backend

3. Configure o ambiente Docker:
   ```bash
   cd docker-compose
   docker-compose up --build

4. A aplicação estará disponível em http://localhost:5000 ou https://localhost:5001

5. URL do Swagger: http://localhost:5000/swagger ou https://localhost:5001/swagger

6. URL do Healthcheck da API: http://localhost:5000/health ou https://localhost:5001/health

7. Caso deseje testar via postman com dados fake importe o arquivo API QuickFood.postman_collection.json do diretorio "postman" na aplicação postman local.

## Autores

- **Anderson Lopez de Andrade RM: 350452** <br>
- **Augusto Rocha Carneiro Napoleão RM: 352125** <br>
- **Henrique Alonso Vicente RM: 354583**<br>

## Documentação Adicional

- **Miro - Domain Storytelling, Context Map, Linguagem Ubíqua e Event Storming**: [Link para o Event Storming](https://miro.com/app/board/uXjVKST91sw=/)
- **Github - Domain Storytelling**: [Link para o Domain Storytelling](https://github.com/QuickFoodFiap/quickfood-domain-story-telling)
- **Github - Context Map**: [Link para o Domain Storytelling](https://github.com/QuickFoodFiap/quickfood-context-map)
- **Github - Linguagem Ubíqua**: [Link para o Domain Storytelling](https://github.com/QuickFoodFiap/quickfood-ubiquitous-language)
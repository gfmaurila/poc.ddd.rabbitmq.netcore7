# Estrutura da API
- .NET Core 7: Framework para desenvolvimento da Microsoft.
- AutoMapper: Biblioteca para realizar mapeamento entre objetos.
- Swagger: Documentação para a API.
- DDD: Domain Drive Design modelagem de desenvolvimento orientado a injeção de dependência.
- Entity FrameWork
- Dapper
- XUnit
- FluentValidator
- Azure.Identity
- MongoDb
- Redis
- Serilog
- Health check
- RabbitMQ


# Descrição 
- Desenvolvi uma API em .NET Core 7 que foca na simplicidade e eficiência, com SQL Server para gerenciamento de dados, Redis para cache, RabbitMQ para mensageria, e MongoDB para armazenamento de logs de erros.

- A API funciona da seguinte forma: Quando uma consulta é feita, primeiro procuramos os dados no Redis, que é rápido devido ao seu armazenamento de dados em memória. Se os dados não estão no Redis, procuramos no SQL Server. Ao mesmo tempo, enviamos uma mensagem através do RabbitMQ para atualizar o Redis com os dados do SQL Server. Assim, na próxima consulta, os dados já estarão disponíveis no Redis.

- Para segurança, implementamos a validação de usuários através de tokens, garantindo que apenas usuários autenticados possam acessar recursos específicos.

- Os logs de erros são armazenados no MongoDB. Isso nos ajuda a monitorar e corrigir problemas de maneira eficaz.

- Finalmente, utilizamos Docker Compose para contêinerizar a API e suas dependências. Isso facilita o deploy e a execução da API em diferentes ambientes.

- No geral, com esta arquitetura, alcançamos uma API simples, eficiente e fácil de manter, que oferece alto desempenho e confiabilidade.


## Autor

- Guilherme Figueiras Maurila

[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila-58250026/)](https://www.linkedin.com/in/guilherme-maurila-58250026/)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)

# 💰 Gestão Financeira API

API REST em **.NET 8** (Minimal API) para gerenciamento financeiro pessoal. Permite cadastrar usuários, controlar o saldo de cada um e registrar despesas, com persistência em **SQLite** via **Entity Framework Core**.

> 🚧 Projeto em desenvolvimento. Veja a seção [Roadmap](#-roadmap) para o que ainda está por vir.

## ✨ Funcionalidades

- Cadastro, consulta, listagem e remoção de usuários
- Atualização de saldo (crédito/débito) com validação para impedir saldo negativo
- Registro de despesas vinculadas a um usuário (camada de serviço/repositório pronta)
- Tratamento global de exceções com respostas JSON padronizadas
- Documentação interativa com **Swagger UI**
- Banco SQLite criado automaticamente na primeira execução

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
| --- | --- | --- |
| .NET / ASP.NET Core | 8.0 | Framework principal (Minimal API) |
| Entity Framework Core | 8.0.0 | ORM |
| EF Core SQLite | 8.0.0 | Provedor de banco de dados |
| Swashbuckle.AspNetCore | 8.0.0 | Swagger / OpenAPI |

## 🏗️ Arquitetura

O projeto segue uma separação em camadas, inspirada em Clean Architecture:

```
GestaoFinanceiraApi/
├── API/                    # Camada de apresentação
│   ├── ApiRest.cs          # Mapeamento dos endpoints
│   └── Gateways/           # Middlewares
├── Application/            # Regras de aplicação
│   ├── DTOs/               # Records de request/response
│   └── Services/           # UsuarioService, DespesaService
├── Domain/
│   └── Entities/           # Usuario, Despesa
├── Infrastructure/
│   ├── Connection/         # AppDbContext (EF Core)
│   └── Repositories/       # UsuarioRepository, DespesaRepository
├── Properties/             # launchSettings.json
├── ExceptionMiddleware.cs  # Tratamento de erros de validação (400)
└── Program.cs              # Configuração e inicialização
```

**Fluxo de uma requisição:** `Endpoint (ApiRest)` → `Service` → `Repository` → `AppDbContext (SQLite)`

## 📦 Modelo de dados

**Usuario**

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | int | Identificador |
| `nome` | string | Nome do usuário |
| `email` | string | E-mail do usuário |
| `saldo` | decimal | Saldo atual (nunca pode ser negativo) |

**Despesa**

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | int | Identificador |
| `descricao` | string | Descrição da despesa |
| `valor` | decimal(18,2) | Valor da despesa |
| `dataCriacao` | DateTime | Data de criação |
| `usuarioId` | int? | Usuário dono da despesa |

Relacionamento: um **Usuário** possui várias **Despesas** (1:N).

## 🚀 Como executar

### Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior

### Passo a passo

```bash
# 1. Clone o repositório
git clone https://github.com/fragad3v/GestaoFinanceiraApi.git
cd GestaoFinanceiraApi

# 2. Restaure as dependências
dotnet restore

# 3. Execute a aplicação
dotnet run
```

A API ficará disponível em:

- HTTP: `http://localhost:5107`
- HTTPS: `https://localhost:7165`

Em ambiente `Development`, a raiz (`/`) redireciona automaticamente para o **Swagger UI** (`/swagger`).

> O arquivo de banco `financas.db` é criado automaticamente na primeira execução (`EnsureCreated`), não sendo necessário rodar migrations.

## 📡 Endpoints

Base URL: `http://localhost:5107`

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` | `/usuarios` | Lista todos os usuários |
| `GET` | `/usuarios/{id}` | Busca um usuário pelo ID |
| `POST` | `/usuarios` | Cria um novo usuário (saldo inicial = 0) |
| `PUT` | `/usuarios/{id}/saldo` | Soma um valor ao saldo do usuário |
| `DELETE` | `/usuarios/{id}` | Remove um usuário |

### Exemplos

**Criar usuário**

```http
POST /usuarios
Content-Type: application/json

{
  "nome": "Maria Silva",
  "email": "maria@email.com"
}
```

Resposta `201 Created`:

```json
{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "saldo": 0
}
```

**Atualizar saldo**

O valor enviado é **somado** ao saldo atual. Use valores negativos para debitar.

```http
PUT /usuarios/1/saldo
Content-Type: application/json

{
  "atualizacaoSaldo": 150.50
}
```

Resposta `200 OK`:

```json
{
  "novoSaldo": 150.50
}
```

**Deletar usuário**

```http
DELETE /usuarios/1
```

Resposta `200 OK` (retorna o usuário removido):

```json
{
  "id": 1,
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "saldo": 150.50
}
```

### Códigos de resposta

| Código | Situação |
| --- | --- |
| `200 OK` | Operação realizada com sucesso |
| `201 Created` | Usuário criado |
| `400 Bad Request` | Dados inválidos ou regra de negócio violada (ex.: saldo negativo) |
| `404 Not Found` | Usuário não encontrado |

Exemplo de erro `404`:

```json
{ "erro": "Usuário 99 não encontrado." }
```

Exemplo de erro `400` (saldo ficaria negativo):

```json
{ "mensagem": "O saldo não pode ser negativo!" }
```

## 📏 Regras de negócio

- Todo usuário é criado com **saldo 0**.
- O **saldo não pode ficar negativo**: a entidade lança `InvalidOperationException`, convertida em `400 Bad Request` pelo middleware global.
- Valores monetários de despesas usam precisão `decimal(18,2)`.

## 🗺️ Roadmap

- [ ] Expor os endpoints de **despesas** (a camada de serviço e repositório já existe)
- [ ] Implementar a subtração de valores em despesas (`SubtrairValor`)
- [ ] Validação de entrada (e-mail válido, nome obrigatório, e-mail único)
- [ ] Uso de `async/await` nas operações de banco
- [ ] Testes unitários e de integração
- [ ] Autenticação e autorização
- [ ] Migrations do EF Core no lugar de `EnsureCreated`
- [ ] Dockerfile

## 🤝 Contribuindo

Sugestões e pull requests são bem-vindos! Abra uma issue para discutir mudanças maiores.

## 👤 Autor

Feito por [@fragad3v](https://github.com/fragad3v).

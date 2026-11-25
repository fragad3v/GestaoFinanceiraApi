# 💰 Gestão Financeira API

API REST em **.NET 8** (Minimal API) para gerenciamento financeiro pessoal. Permite cadastrar usuários, controlar o saldo de cada um e registrar despesas, calculando automaticamente o **saldo atualizado** (saldo menos o total de despesas).

Construída com **Entity Framework Core + SQLite**, programação **assíncrona**, DTOs de entrada e saída e organização em camadas inspirada em Clean Architecture.

## ✨ Funcionalidades

- Cadastro, consulta, listagem e remoção de usuários
- Atualização de saldo por valor incremental (positivo ou negativo)
- Registro de despesas vinculadas a um usuário
- Listagem de usuários com suas despesas e **saldo atualizado** calculado
- Validações de regra de negócio (saldo não negativo, valor de despesa positivo, usuário existente)
- Tratamento global de exceções com resposta JSON padronizada
- Documentação interativa com **Swagger UI**
- Banco SQLite criado automaticamente na primeira execução

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
| --- | --- | --- |
| .NET / ASP.NET Core | 8.0 | Framework principal (Minimal API) |
| Entity Framework Core | 8.0.0 | ORM |
| EF Core SQLite | 8.0.0 | Banco de dados |
| Swashbuckle.AspNetCore | 8.0.0 | Swagger / OpenAPI |

## 🏗️ Arquitetura

```
GestaoFinanceiraApi/
├── API/
│   ├── ApiRest.cs               # Mapeamento e handlers dos endpoints
│   └── Gateways/
│       └── ExceptionMiddleware  # Middleware global de erros
├── Application/
│   ├── DTOs/                    # Records de request/response
│   └── Services/                # UsuarioService, DespesaService
├── Domain/
│   └── Entities/                # Usuario, Despesa
├── Infrastructure/
│   ├── Connection/              # AppDbContext (EF Core)
│   └── Repositories/            # UsuarioRepository, DespesaRepository
├── Shared/                      # DateTimeHelper (formatação de datas)
├── Properties/                  # launchSettings.json
└── Program.cs                   # Configuração, DI e inicialização
```

**Fluxo de uma requisição:**

`Endpoint (ApiRest)` → `Service` → `Repository` → `AppDbContext (SQLite)`

A API devolve apenas DTOs: as entidades de domínio não são expostas diretamente nas respostas.

## 📦 Modelo de dados

```
Usuario 1 ──────── N Despesa
```

**Usuario**

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | int | Identificador |
| `nome` | string | Nome do usuário |
| `email` | string | E-mail |
| `saldo` | decimal | Saldo atual (não pode ser negativo) |

**Despesa**

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | int | Identificador |
| `descricao` | string | Descrição da despesa |
| `valor` | decimal(18,2) | Valor da despesa (deve ser maior que zero) |
| `dataCriacao` | string | Data de criação no formato `dd/MM/yyyy HH:mm`, preenchida automaticamente |
| `usuarioId` | int | Usuário dono da despesa (chave estrangeira) |

> Ao remover um usuário, suas despesas também são removidas (exclusão em cascata).

## 🚀 Como executar

### Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior

### Passo a passo

```bash
git clone https://github.com/fragad3v/GestaoFinanceiraApi.git
cd GestaoFinanceiraApi
dotnet restore
dotnet run
```

A API sobe em:

- HTTP: `http://localhost:5107`
- HTTPS: `https://localhost:7165`

Em ambiente `Development`, a raiz (`/`) redireciona para o **Swagger UI** (`/swagger`), onde todos os endpoints podem ser testados.

> O banco `financas.db` é criado automaticamente na primeira execução e não é versionado no Git.

## 📡 Endpoints

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` | `/usuarios` | Lista todos os usuários com despesas e saldo atualizado |
| `GET` | `/usuarios/{id}` | Busca um usuário pelo Id, com o detalhe das despesas |
| `POST` | `/usuarios` | Cria um usuário (saldo inicial = 0) |
| `PUT` | `/usuarios/{id}/saldo` | Soma um valor ao saldo do usuário |
| `DELETE` | `/usuarios/{id}` | Remove um usuário |
| `POST` | `/despesas` | Registra uma despesa para um usuário |

### Exemplos

**Criar usuário** — `POST /usuarios`

```json
{
  "nome": "Maria Silva",
  "email": "maria@email.com"
}
```

Resposta `201 Created`:

```json
{
  "id": 1,
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "saldo": 0
}
```

**Adicionar saldo** — `PUT /usuarios/1/saldo`

O valor enviado é **somado** ao saldo atual. Use um valor negativo para debitar.

```json
{
  "atualizacaoSaldo": 1500.00
}
```

Resposta `200 OK`:

```json
{
  "novoSaldo": 1500.00
}
```

**Registrar despesa** — `POST /despesas`

```json
{
  "descricao": "Supermercado",
  "valor": 230.90,
  "usuarioId": 1
}
```

Resposta `201 Created`:

```json
{
  "nomeUsuario": "Maria Silva",
  "descricao": "Supermercado",
  "valor": 230.90,
  "usuarioId": 1,
  "dataCriacao": "10/01/2026 14:30"
}
```

**Listar usuários** — `GET /usuarios`

Nesta listagem as despesas vêm resumidas, sem ids.

```json
[
  {
    "id": 1,
    "nome": "Maria Silva",
    "email": "maria@email.com",
    "saldo": 1500.00,
    "despesas": [
      {
        "descricao": "Supermercado",
        "valor": 230.90,
        "dataCriacao": "10/01/2026 14:30"
      }
    ],
    "saldoAtualizado": 1269.10
  }
]
```

**Buscar usuário por Id** — `GET /usuarios/1`

Na busca individual cada despesa traz seu próprio `id` e o `usuarioId` do dono.

```json
{
  "id": 1,
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "saldo": 1500.00,
  "despesas": [
    {
      "id": 1,
      "descricao": "Supermercado",
      "valor": 230.90,
      "dataCriacao": "10/01/2026 14:30",
      "usuarioId": 1
    }
  ],
  "saldoAtualizado": 1269.10
}
```

`saldoAtualizado = saldo − soma das despesas`

**Deletar usuário** — `DELETE /usuarios/1`

Resposta `200 OK` (retorna o usuário removido):

```json
{
  "id": 1,
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "saldo": 1500.00
}
```

### Respostas de erro

| Código | Situação | Exemplo de corpo |
| --- | --- | --- |
| `400 Bad Request` | Regra de negócio violada (saldo negativo, valor de despesa ≤ 0, usuário inexistente ao registrar despesa) | `{ "status": 400, "mensagem": "O saldo não pode ser negativo!" }` |
| `404 Not Found` | Usuário não encontrado (busca, atualização de saldo e remoção) | `{ "erro": "Usuário 99 não encontrado." }` |
| `500 Internal Server Error` | Erro inesperado | `{ "status": 500, "mensagem": "Ocorreu um erro interno no servidor." }` |

## 📏 Regras de negócio

- Todo usuário é criado com **saldo 0**.
- O **saldo não pode ficar negativo**: a regra é aplicada na própria entidade `Usuario`.
- O **valor de uma despesa deve ser maior que zero**.
- Só é possível registrar despesa para um **usuário existente** (caso contrário a API responde `400`).
- A data da despesa é registrada automaticamente, no formato `dd/MM/yyyy HH:mm`, com o horário do servidor.
- O **saldo atualizado** é calculado na consulta (`saldo − total de despesas`). Registrar uma despesa não altera o saldo armazenado.

## 🗺️ Roadmap (v2)

- [ ] Atualizar e remover despesa (`PUT/DELETE /usuarios/{usuarioId}/despesas/{despesaId}`)
- [ ] Listar despesas de um usuário com filtros por período
- [ ] Validação de entrada (nome obrigatório, e-mail válido e único)
- [ ] Interfaces nos repositórios (inversão de dependência)
- [ ] Testes unitários e de integração (xUnit)
- [ ] Categorias de despesa
- [ ] Migrations do EF Core
- [ ] Autenticação com JWT
- [ ] Docker e CI com GitHub Actions

## 👤 Autor

Feito por [@fragad3v](https://github.com/fragad3v).

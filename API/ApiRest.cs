using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Application.Services;

namespace GestaoFinanceiraApi.API
{
    public class ApiRest
    {
        public void MapEndpoints(WebApplication app)
        {
            var usuarios = app.MapGroup("/usuarios").WithTags("Usuários");

            usuarios.MapGet("/", ListarTodosUsuariosAsync)
                .WithSummary("Listar usuários")
                .WithDescription("Retorna todos os usuários com suas despesas e o saldo atualizado.")
                .Produces<List<ListarUsuarioResponseDto>>(StatusCodes.Status200OK);

            usuarios.MapGet("/{id}", BuscarUsuarioPorIdAsync)
                .WithSummary("Buscar usuário por Id")
                .WithDescription("Retorna o usuário com o detalhe de cada despesa.")
                .Produces<BuscarUsuarioResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            usuarios.MapPost("/", AdicionarUsuarioAsync)
                .WithSummary("Criar usuário")
                .WithDescription("Cria um usuário com saldo inicial 0.")
                .Produces<CriarUsuarioResponseDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            usuarios.MapPut("/{id}/saldo", AtualizarSaldoAsync)
                .WithSummary("Atualizar saldo")
                .WithDescription("Soma o valor informado ao saldo. Use valor negativo para debitar. O saldo não pode ficar negativo.")
                .Produces<AtualizarSaldoResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            usuarios.MapDelete("/{id}", DeletarUsuarioAsync)
                .WithSummary("Remover usuário")
                .WithDescription("Remove o usuário e todas as suas despesas.")
                .Produces<DeletarUsuarioResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            var despesas = app.MapGroup("/despesas").WithTags("Despesas");

            despesas.MapPost("/", AdicionarDespesaAsync)
                .WithSummary("Registrar despesa")
                .WithDescription("Registra uma despesa para um usuário existente. O valor deve ser maior que zero.")
                .Produces<CriarDespesaResponseDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }

        [EndpointDescription("Buscar usuário por Id.")]
        private async static Task<IResult> BuscarUsuarioPorIdAsync(int id, UsuarioService usuarioService)
        {
            var usuario = await usuarioService.BuscarUsuarioPorIdAsync(id);
            return usuario is null 
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(usuario);
        }

        [EndpointDescription("Adicionar usuário.")]
        private async static Task<IResult> AdicionarUsuarioAsync(CriarUsuarioRequestDto dto, UsuarioService usuarioService)
        {
            var response = await usuarioService.CriarUsuarioAsync(dto);
            return response is null
                ? Results.BadRequest(new { erro = "Não foi possível criar o usuário." })
                : Results.Created($"/usuarios/{response.Id}", response);
        }

        [EndpointDescription("Adicionar saldo.")]
        private async static Task<IResult> AtualizarSaldoAsync(int id, AtualizarSaldoRequestDto dto, UsuarioService usuarioService)
        {
            var response = await usuarioService.AtualizarSaldoAsync(id, dto);
            return response is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(response);
        }

        [EndpointDescription("Deletar usuário por Id.")]
        private async static Task<IResult> DeletarUsuarioAsync(int id, UsuarioService usuarioService)
        {
            var usuarioDeletado = await usuarioService.DeletarUsuarioAsync(id);
            return usuarioDeletado is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(new DeletarUsuarioResponseDto(usuarioDeletado.Id, usuarioDeletado.Nome, usuarioDeletado.Email, usuarioDeletado.Saldo));
        }

        [EndpointDescription("Listar todos usuários.")]
        private async static Task<IResult> ListarTodosUsuariosAsync(UsuarioService usuarioService)
        {
            var usuarios = await usuarioService.ListarTodosUsuariosAsync();
            return Results.Ok(usuarios);
        }

        [EndpointDescription("Adicionar despesa.")]
        private async static Task<IResult> AdicionarDespesaAsync(CriarDespesaRequestDto dto, DespesaService despesaService)
        {
            var despesas = await despesaService.AdicionarDespesaAsync(dto);
            return despesas is null
                ? Results.NotFound(new { erro = $"Não foi possível criar a despesa." })
                : Results.Created($"/usuarios/{dto.UsuarioId}", despesas);
        }
    }
}
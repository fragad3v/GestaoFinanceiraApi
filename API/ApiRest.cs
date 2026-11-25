using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Application.Services;
using GestaoFinanceiraApi.Domain.Entities;

namespace GestaoFinanceiraApi.API
{
    public class ApiRest
    {   
        public void MapEndpoints(WebApplication app)
        {
            app.MapGet("/usuarios", ListarTodosUsuariosAsync);
            app.MapGet("/usuarios/{id}", BuscarUsuarioPorIdAsync);
            app.MapPost("/usuarios", AdicionarUsuarioAsync);
            app.MapPut("/usuarios/{id}/saldo", AtualizarSaldoAsync);
            app.MapDelete("/usuarios/{id}", DeletarUsuarioAsync);
            app.MapPost("/despesas", AdicionarDespesaAsync);
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
                ? Results.NotFound(new { erro = $"Usuário {dto.UsuarioId} não encontrado." })
                : Results.Created($"/usuarios/{dto.UsuarioId}", despesas);
        }
    }
}
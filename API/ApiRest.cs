using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Application.Services;

namespace GestaoFinanceiraApi.API
{
    public class ApiRest
    {   
        public void MapEndpoints(WebApplication app)
        {
            app.MapGet("/usuarios", ListarTodosUsuarios);
            app.MapGet("/usuarios/{id}", BuscarUsuarioPorId);
            app.MapPost("/usuarios", AdicionarUsuario);
            app.MapPut("/usuarios/{id}/saldo", AtualizarSaldo);
            app.MapDelete("/usuarios/{id}", DeletarUsuario);
            app.MapPost("/despesas", AdicionarDespesa);
        }

        [EndpointDescription("Buscar usuário por Id.")]
        private static IResult BuscarUsuarioPorId(int id, UsuarioService usuarioService)
        {
            var usuario = usuarioService.BuscarUsuarioPorId(id);

            return usuario is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(usuario);
        }

        [EndpointDescription("Adicionar usuário.")]
        private static IResult AdicionarUsuario(
            CriarUsuarioRequestDto dto, UsuarioService usuarioService)
        {
            var response = usuarioService.CriarUsuario(dto);

            return response is null
                ? Results.BadRequest(new { erro = "Não foi possível criar o usuário." })
                : Results.Created("/usuarios", response);
        }

        [EndpointDescription("Adicionar saldo.")]
        private static IResult AtualizarSaldo(
            int id, AtualizarSaldoRequestDto dto, UsuarioService usuarioService)
        {
            var response = usuarioService.AtualizarSaldo(id, dto);

            return response is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(response);
        }

        [EndpointDescription("Deletar usuário por Id.")]
        private static IResult DeletarUsuario(int id, UsuarioService usuarioService)
        {
            var usuarioDeletado = usuarioService.DeletarUsuario(id);
            return usuarioDeletado is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(new DeletarUsuarioResponseDto(usuarioDeletado.Id, usuarioDeletado.Nome, usuarioDeletado.Email, usuarioDeletado.Saldo));
        }

        [EndpointDescription("Listar todos usuários.")]
        private static IResult ListarTodosUsuarios(UsuarioService usuarioService)
        {
            var usuarios = usuarioService.ListarTodosUsuarios();
            return Results.Ok(usuarios);
        }

        [EndpointDescription("Adicionar despesa.")]
        private static IResult AdicionarDespesa(
            CriarDespesaRequestDto dto, DespesaService despesaService)
        {
            var response = despesaService.AdicionarDespesa(dto);

            return response is null
                ? Results.BadRequest(new { erro = "Não foi possível criar a despesa." })
                : Results.Created("/despesas", response);
        }
    }
}
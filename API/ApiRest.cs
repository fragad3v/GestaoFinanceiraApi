using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Application.Services;

namespace GestaoFinanceiraApi.API
{
    public class ApiRest(WebApplication app)
    {
        public void MapEndpoints()
        {
            app.MapGet("/usuarios/{id}", GetUsuarioById);
            app.MapPost("/usuarios", AdicionarUsuario);
            app.MapPut("/usuarios/{id}/saldo", AtualizarSaldo);
        }

        private static IResult GetUsuarioById(int id, UsuarioService usuarioService)
        {
            var usuario = usuarioService.GetUsuarioById(id);

            return usuario is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(usuario);
        }

        private static IResult AdicionarUsuario(
            CriarUsuarioRequestDto dto, UsuarioService usuarioService)
        {
            var response = usuarioService.CriarUsuario(dto);

            return response is null
                ? Results.BadRequest(new { erro = "Não foi possível criar o usuário." })
                : Results.Created("/usuarios", response);
        }

        private static IResult AtualizarSaldo(
            int id, AtualizarSaldoRequestDto dto, UsuarioService usuarioService)
        {
            var response = usuarioService.AtualizarSaldo(id, dto);

            return response is null
                ? Results.NotFound(new { erro = $"Usuário {id} não encontrado." })
                : Results.Ok(response);
        }
    }
}
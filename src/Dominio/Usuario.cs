using CSharpFunctionalExtensions;

namespace Dominio
{
    public class Usuario
    {
        private Usuario(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public static Result<Usuario> Criar(string nome, string email)
        {
            Result validacao = Result.Combine(
                Result.FailureIf(string.IsNullOrEmpty(nome), "Nome é obrigatório."),
                Result.FailureIf(string.IsNullOrEmpty(email), "E-Email é obrigatório.")
            );

            return Result.SuccessIf(
                validacao.IsSuccess,
                new Usuario(nome, email),
                validacao.IsFailure ? validacao.Error : String.Empty);
        }

        public Result AtualizarEmail(string email)
        {
            return Result.FailureIf(email == email.Split('@')[1], "E-mail inválido.")
            .Tap(() => 
            {
                Email = email;
            })
            .Finally(resultado => resultado);
        }
    }
}
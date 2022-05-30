using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; private set; }
        public Nome Nome { get; private set; }
        public Email Email { get; private set; }

        public Usuario (Nome nome, Email email)
        {
            Nome = nome;
            Email = email;
        }

        public void AtualizarEmail(Email email)
        {
            Email = email;
        }
    }

    public class Email
    {
        private readonly string _value;

        private Email(string value)
        {
            _value = value;
        }

        public static Result<Email> Criar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<Email>("O e-mail não pode ficar vazio.");

            if (email.Length > 100)
                return Result.Failure<Email>("O e-mail é muito longo.");

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                return Result.Failure<Email>("E-mail inválido.");

            return Result.Success(new Email(email));
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }

    public class Nome
    {
        private readonly string _value;

        private Nome(string value)
        {
            _value = value;
        }

        public static Result<Nome> Criar(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Nome>("Name não pode ficar vazio.");

            if (name.Length > 50)
                return Result.Failure<Nome>("Nome é muito longo.");

            return Result.Success(new Nome(name));
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Dominio
{
    public class Cliente
    {
        public int Id { get; private set; }
        public Nome Nome { get; private set; }
        public Email Email { get; private set; }
        public bool Negativado { get; private set; }

        public Cliente (Nome nome, Email email, bool negativado = default, int id = default)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Negativado = negativado;
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

        public override bool Equals(object? email)
        {
            return _value.Equals(email);
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

    public class Cidade
    {
        private readonly string _value;

        private Cidade(string value)
        {
            _value = value;
        }

        public static Result<Cidade> Criar(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Cidade>("Name não pode ficar vazio.");

            return Result.Success(new Cidade(name));
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }

    public class Estado
    {
        private readonly string _value;

        private Estado(string value)
        {
            _value = value;
        }

        public static Result<Estado> Criar(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Estado>("Name não pode ficar vazio.");

            return Result.Success(new Estado(name));
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
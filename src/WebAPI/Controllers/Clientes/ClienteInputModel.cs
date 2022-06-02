using CSharpFunctionalExtensions;
using Dominio;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers.Clientes
{
    public class ClienteInputModel
    {
        [Required]
        public string Nome { get; set; }

        [Required, Email]
        public string Email { get; set; }

        [Required]
        public CidadeEstadoInputModel CidadeEstado { get; set; }

        public partial class CidadeEstadoInputModel
        {
            [Required]
            public string Cidade { get; set; }

            [Required]
            public string Estado { get; set; }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string email)
                return new ValidationResult("Teste.");

            Result<Email> criar = Email.Criar(email);
    
            if (criar.IsFailure)
                return new ValidationResult("Teste.");
    
            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CidadeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string cidade)
                return new ValidationResult("Teste.");

            Result<Cidade> criar = Cidade.Criar(cidade);

            if (criar.IsFailure)
                return new ValidationResult("Teste.");

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EstadoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not string estado)
                return new ValidationResult("Teste.");

            Result<Estado> criar = Estado.Criar(estado);

            if (criar.IsFailure)
                return new ValidationResult("Teste.");

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CidadeEstadoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            return ValidationResult.Success;
        }
    }
}

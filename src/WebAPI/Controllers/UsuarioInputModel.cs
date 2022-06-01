using CSharpFunctionalExtensions;
using Dominio;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    public class UsuarioInputModel
    {
        public string Nome { get; set; }

        [Email]
        public string Email { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
    
            string email = value as string;
            if (email == null)
                return new ValidationResult("" /*Errors.General.ValueIsInvalid().Serialize()*/);

            Result<Email> emailResult = Email.Criar(email);
    
            if (emailResult.IsFailure)
                    return new ValidationResult("" /*emailResult.Error.Serialize()*/);
    
            return ValidationResult.Success;
        }
    }
}

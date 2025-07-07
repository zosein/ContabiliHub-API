using System.Text.RegularExpressions;
using ContabiliHub.Application.DTOs;

namespace ContabiliHub.Application.Validators
{
    public class UsuarioLoginDtoValidator : IValidator<UsuarioLoginDto>
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled
        );

        public ValidationResult Validate(UsuarioLoginDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("E-mail é obrigatório.");
            else if (!EmailRegex.IsMatch(dto.Email))
                errors.Add("E-mail deve ter um formato válido.");

            if (string.IsNullOrWhiteSpace(dto.Senha))
                errors.Add("Senha é obrigatória.");

            return errors.Count == 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(errors);
        }
    }
}
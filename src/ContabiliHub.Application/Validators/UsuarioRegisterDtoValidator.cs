using System.Text.RegularExpressions;
using ContabiliHub.Application.DTOs;

namespace ContabiliHub.Application.Validators
{
    public class UsuarioRegisterDtoValidator : IValidator<UsuarioRegisterDto>
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled
        );

        public ValidationResult Validate(UsuarioRegisterDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.NomeCompleto))
                errors.Add("Nome completo é obrigatório.");
            else if (dto.NomeCompleto.Length < 2)
                errors.Add("Nome completo deve ter pelo menos 2 caracteres.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("E-mail é obrigatório.");
            else if (!EmailRegex.IsMatch(dto.Email))
                errors.Add("E-mail deve ter um formato válido.");

            if (string.IsNullOrWhiteSpace(dto.Senha))
                errors.Add("Senha é obrigatória.");
            else if (dto.Senha.Length < 6)
                errors.Add("Senha deve ter pelo menos 6 caracteres.");

            return errors.Count == 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(errors);
        }
    }
}
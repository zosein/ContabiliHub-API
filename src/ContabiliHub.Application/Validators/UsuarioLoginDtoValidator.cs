using ContabiliHub.Application.DTOs;
using FluentValidation;

namespace ContabiliHub.Application.Validators
{
    public class UsuarioLoginDtoValidator : AbstractValidator<UsuarioLoginDto>
    {
        public UsuarioLoginDtoValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail deve ter um formato válido.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        }
    }
}
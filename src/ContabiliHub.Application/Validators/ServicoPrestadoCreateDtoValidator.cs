using System.Data;
using ContabiliHub.Application.DTOs;
using FluentValidation;

namespace ContabiliHub.Application.Validators
{
    public class ServicoPrestadoCreateDtoValidator : AbstractValidator<ServicoPrestadoCreateDto>
    {
        public ServicoPrestadoCreateDtoValidator()
        {
            RuleFor(s => s.ClienteId)
                .NotEmpty().WithMessage("ClienteId é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("ClienteId não pode ser um Guid.Empty.");

            RuleFor(s => s.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MinimumLength(5).WithMessage("Descrição deve ter pelo menos 5 caracteres.")
                .MaximumLength(200).WithMessage("Descrição não pode ter mais de 200 caracteres.");

            RuleFor(s => s.Valor)
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");

            RuleFor(s => s.DataPrestacao)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Data não pode ser futura.");
        }
    }
}
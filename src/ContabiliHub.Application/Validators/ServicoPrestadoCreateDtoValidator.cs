using ContabiliHub.Application.DTOs;

namespace ContabiliHub.Application.Validators
{
    public class ServicoPrestadoCreateDtoValidator : IValidator<ServicoPrestadoCreateDto>
    {
        public ValidationResult Validate(ServicoPrestadoCreateDto dto)
        {
            var errors = new List<string>();

            if (dto.ClienteId == Guid.Empty)
                errors.Add("ClinteId é obrigatório e não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                errors.Add("Descrição é obrigatória.");
            else if (dto.Descricao.Length < 5 || dto.Descricao.Length > 200)
                errors.Add("Descrição deve ter entre 5 e 200 caracteres.");

            if (dto.Valor <= 0)
                errors.Add("Valor deve ser maior que zero.");

            if (dto.DataPrestacao > DateTime.Now)
                errors.Add("Data de prestação não pode ser futura.");

            return errors.Count == 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(errors);
        }
    }
}
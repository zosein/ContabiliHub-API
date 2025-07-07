using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.DTOs
{
    public record ServicoPrestadoReadDto(
        Guid Id,
        string Descricao,
        decimal Valor,
        DateTime DataPrestacao,
        bool Pago,
        Guid ClienteId,
        string? NomeCliente
    )
    {
        //Construtor para conversão direta da entidade
        public ServicoPrestadoReadDto(ServicoPrestado servico)
            : this(
                servico.Id,
                servico.Descricao,
                servico.Valor,
                servico.DataPrestacao,
                servico.Pago,
                servico.ClienteId,
                servico.Cliente?.NomeCompleto
            )
        {
        }
    }
}
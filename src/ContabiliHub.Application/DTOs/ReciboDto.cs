using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.DTOs
{
    public record ReciboDto(
        string NomeCliente,
        string DescricaoServico,
        decimal Valor,
        DateTime DataPrestacao,
        DateTime DataEmissao,
        string AssinaturaResponsavel = "__________________"
    )
    {
        public ReciboDto(ServicoPrestado servico) : this(
            servico.Cliente?.NomeCompleto ?? string.Empty,
            servico.Descricao,
            servico.Valor,
            servico.DataPrestacao,
            DateTime.UtcNow
        )
        { }
    }
}
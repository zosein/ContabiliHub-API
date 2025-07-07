using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.DTOs
{
    public record ServicoPrestadoCreateDto(
        Guid ClienteId,
        string Descricao,
        decimal Valor,
        DateTime DataPrestacao,
        bool Pago = false
    )
    {
        //Método para converter para entidade
        public ServicoPrestado ToEntity() => new()
        {
            ClienteId = this.ClienteId,
            Descricao = this.Descricao,
            Valor = this.Valor,
            DataPrestacao = this.DataPrestacao,
            Pago = this.Pago
        };

        //Método para aplicar em entidade existente
        public void ApplyTo(ServicoPrestado servico)
        {
            servico.ClienteId = this.ClienteId;
            servico.Descricao = this.Descricao;
            servico.Valor = this.Valor;
            servico.DataPrestacao = this.DataPrestacao;
            servico.Pago = this.Pago;
        }
    }
}
namespace ContabiliHub.Application.DTOs
{
    public class ServicoPrestadoCreateDto
    {
        public Guid ClienteId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPrestacao { get; set; }
        public bool Pago { get; set; } = false;
    }
}
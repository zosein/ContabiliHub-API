namespace ContabiliHub.Domain.Entities

{
    public class ServicoPrestado
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }

        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPrestacao { get; set; } = DateTime.UtcNow;
        public bool Pago { get; set; } = false;

        public Cliente? Cliente { get; set; }
    }
}
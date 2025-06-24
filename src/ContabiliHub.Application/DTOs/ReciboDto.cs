namespace ContabiliHub.Application.DTOs
{
    public class ReciboDto
    {
        public string NomeCliente { get; set; } = string.Empty;
        public string DescricaoServico { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPrestacao { get; set; }
        public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
        public string AssinaturaResponsavel { get; set; } = "____________________";
    }
}
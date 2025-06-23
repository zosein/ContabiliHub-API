namespace ContabiliHub.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
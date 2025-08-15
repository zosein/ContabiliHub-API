using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContabiliHub.Domain.Entities;

namespace ContabiliHub.Application.DTOs
{
    public record ClienteReadDto(
        Guid Id,
        string NomeCompleto,
        string CpfMascarado,
        string Email,
        string Telefone,
        string Endereco,
        DateTime DataCadastro
    )
    {
        public static ClienteReadDto FromEntity(Cliente cliente) =>
            new(
                cliente.Id,
                cliente.NomeCompleto,
                MascararCpf(cliente.CPF),
                cliente.Email,
                cliente.Telefone,
                cliente.Endereco,
                cliente.DataCadastro

            );

        private static string MascararCpf(string cpfSomenteDigito)
        {
            var digito = new string(cpfSomenteDigito.Where(char.IsDigit).ToArray());
            if (digito.Length != 11)
                return cpfSomenteDigito; // Retorna o CPF original se não tiver 11 dígitos

            return $"{digito[..3]}.***.***-{digito[^2]}";
        }
    }


}
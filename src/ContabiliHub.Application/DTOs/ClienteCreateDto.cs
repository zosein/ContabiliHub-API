using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContabiliHub.Application.DTOs
{
    public record ClienteCreateDto(
        string NomeCompleto,
        string Cpf,
        string Email,
        string Telefone,
        string Endereco
    );
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContabiliHub.Application.DTOs
{
    public class ResultadoPaginado<T>
    {
        public IEnumerable<T> Itens { get; }
        public int Pagina { get; }
        public int PaginaTamanho { get; }
        public int TotalItens { get; }
        public int PaginasTotal => (int)Math.Ceiling((double)TotalItens / PaginaTamanho);

        public ResultadoPaginado(IEnumerable<T> itens, int pagina, int paginaTamanho, int totalItens)
        {
            Itens = itens;
            Pagina = pagina;
            PaginaTamanho = paginaTamanho;
            TotalItens = totalItens;
        }

    }
}
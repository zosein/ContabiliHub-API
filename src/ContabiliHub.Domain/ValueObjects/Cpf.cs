using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ContabiliHub.Domain.ValueObjects
{
    //Value Objects simplificado p/ validação de cpf
    public readonly struct Cpf
    {
        private static readonly Regex CpfSomenteDigitos = new(@"^\d{11}$", RegexOptions.Compiled);
        public string Valor { get; }

        public Cpf(string valor) => Valor = valor;

        public static Cpf Create(string limpo)
        {
            var normalizado = Normalizar(limpo);
            if (!isValido(normalizado))
                throw new ArgumentException("CPF inválido.", nameof(limpo));
            return new Cpf(normalizado);
        }

        public static string Normalizar(string limpo) =>
            new(limpo.Where(char.IsDigit).ToArray());

        public static bool isValido(string digito)
        {
            if (!CpfSomenteDigitos.IsMatch(digito))
                return false;

            //elimina cpfs com todos digitos iguais
            if (digito.Distinct().Count() == 1)
                return false;

            //calculo digito verificadores
            bool ChecaDigito(int tamanho)
            {
                int soma = 0;
                for (int i = 0; i < tamanho; i++)
                    soma += (tamanho + 1 - i) * (digito[i] - '0');

                int resto = soma % 11;
                return digito[tamanho] - '0' == (resto < 2 ? 0 : 11 - resto);
            }

            return ChecaDigito(10) && ChecaDigito(11);
        }

        public override string ToString() => Valor;
        public static implicit operator string(Cpf cpf) => cpf.Valor;

    }
}
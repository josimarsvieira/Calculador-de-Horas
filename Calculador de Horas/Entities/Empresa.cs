namespace CalculadorDeHoras.Entities
{
    /// <summary>
    /// Representa uma empresa.
    /// </summary>
    public class Empresa
    {
        /// <summary>
        /// Metodo construtor da classe
        /// </summary>
        /// <param name="razaoSocial">Razão Social.</param>
        /// <param name="cNPJ">CNPJ.</param>
        /// <param name="diaFechamento">Dia do fechamento da folha de pagamento.</param>
        public Empresa(string razaoSocial, string cNPJ, int diaFechamento)
        {
            RazaoSocial = razaoSocial;
            CNPJ = cNPJ;
            DiaFechamento = diaFechamento;
        }

        /// <summary>
        /// CNPJ.
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        /// Dia de fechamento da folha de pagamento.
        /// </summary>
        public int DiaFechamento { get; set; }

        /// <summary>
        /// Identificador.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Razão Social.
        /// </summary>
        public string RazaoSocial { get; set; }
    }
}
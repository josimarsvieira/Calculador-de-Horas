using System;

namespace CalculadorDeHoras.Entities
{
    /// <summary>
    /// Classe responsavel pelos registros do Banco de Hora.
    /// </summary>
    public class BancoDeHoras
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BancoDeHoras()
        {
        }

        /// <summary>
        /// Banco de horas do funcionario.
        /// </summary>
        /// <param name="horasExtras">TimeSpan contendo a quantidade de horas extras do funcionário</param>
        /// <param name="justificativa">String de justificativa da inclusão</param>
        /// <param name="dataRegistro">Data do registro</param>
        /// <param name="funcionarioId">Número do registro do funcionário</param>
        public BancoDeHoras(TimeSpan horasExtras, string justificativa, DateTime dataRegistro, int funcionarioId)
        {
            HorasExtras = horasExtras;
            Justificativa = justificativa;
            DataRegistro = dataRegistro;
            Funcionario = funcionarioId;
        }

        /// <summary>
        /// Dia do registro.
        /// </summary>
        public DateTime DataRegistro { get; set; }

        /// <summary>
        /// Registro do funcionario
        /// </summary>
        public int Funcionario { get; set; }

        /// <summary>
        /// TimeSpan contendo as horas extras do funcionario.
        /// </summary>
        public TimeSpan HorasExtras { get; set; }

        /// <summary>
        /// Id gerada pelo BD.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// String contendo a justificativa para adição.
        /// </summary>
        public string Justificativa { get; set; }
    }
}
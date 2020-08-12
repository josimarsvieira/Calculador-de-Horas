using System;

namespace CalculadorDeHoras.Entities
{
    /// <summary>
    /// Classe para registro de horas do funcionario.
    /// </summary>
    public class HorasFuncionario
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public HorasFuncionario()
        {
        }

        /// <summary>
        /// Contrutor para objeto horas do funcionario.
        /// </summary>
        /// <param name="entrada">Hora de entrada.</param>
        /// <param name="saida">Hora de saída.</param>
        /// <param name="horaSaidaAlmoco">Hora de saida para almoço.</param>
        /// <param name="horaRetornoAlmoco">Hora de retorno do almoço.</param>
        /// <param name="funcionario">Numero do registro do funcionário.</param>
        /// <param name="dataRegistro">Data do registro.</param>
        public HorasFuncionario(TimeSpan entrada, TimeSpan saida, TimeSpan horaSaidaAlmoco, TimeSpan horaRetornoAlmoco, int funcionario, DateTime dataRegistro)
        {
            Entrada = entrada;
            Saida = saida;
            HoraSaidaAlmoco = horaSaidaAlmoco;
            HoraRetornoAlmoco = horaRetornoAlmoco;
            Funcionario = funcionario;
            DataRegistro = dataRegistro;
        }

        /// <summary>
        /// Data do registro.
        /// </summary>
        public DateTime DataRegistro { get; set; }

        /// <summary>
        /// Hora de entrada no trabalho.
        /// </summary>
        public TimeSpan Entrada { get; set; }

        /// <summary>
        /// Horas extras feitas no dia.
        /// </summary>
        public TimeSpan Extras { get; set; }

        /// <summary>
        /// Id do funcionario vinculado ao registro.
        /// </summary>
        public int Funcionario { get; set; }

        /// <summary>
        /// Hora de retorno do Almoço
        /// </summary>
        public TimeSpan HoraRetornoAlmoco { get; set; }

        /// <summary>
        /// Hora de saida para o Almoço
        /// </summary>
        public TimeSpan HoraSaidaAlmoco { get; set; }

        /// <summary>
        /// Id gerado pelo BD.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Hora de saida do trabalho.
        /// </summary>
        public TimeSpan Saida { get; set; }

        /// <summary>
        /// Metodo para calcular as horas extras.
        /// </summary>
        /// <param name="totalHorasParaTrabalhar">Total de horas obrigatorias para trabalho diario, conforme contrato.</param>
        public TimeSpan CalculaExtras(TimeSpan totalHorasParaTrabalhar)
        {
            Extras = Saida.Subtract(HoraRetornoAlmoco).Add(HoraSaidaAlmoco.Subtract(Entrada)).Subtract(totalHorasParaTrabalhar);
            return Extras;
        }
    }
}
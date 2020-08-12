using System;

namespace CalculadorDeHoras.Entities
{
    /// <summary>
    /// Classe para registro de funcionario.
    /// </summary>
    public class Funcionario
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public Funcionario()
        {
        }

        /// <summary>
        /// Construtor do objeto Funcionário.
        /// </summary>
        /// <param name="registro">Número do registro do funcionário.</param>
        /// <param name="nome">Nome.</param>
        /// <param name="funcao">Função.</param>
        /// <param name="horaIncio">Hora de início da jornada.</param>
        /// <param name="horaTermino">Hora de término da jornada.</param>
        /// <param name="horaSaidaAlmoco">Hora de saída para almoço.</param>
        /// <param name="horaRetornoAlmoco">Hora de retorno do almoço.</param>
        /// <param name="ativo"></param>
        public Funcionario(int registro, string nome, string funcao, TimeSpan horaIncio, TimeSpan horaTermino, TimeSpan horaSaidaAlmoco, TimeSpan horaRetornoAlmoco, bool ativo)
        {
            Registro = registro;
            Nome = nome;
            Funcao = funcao;
            HoraInicio = horaIncio;
            HoraTermino = horaTermino;
            HoraSaidaAlmoco = horaSaidaAlmoco;
            HoraRetornoAlmoco = horaRetornoAlmoco;
            Ativo = ativo;
        }

        /// <summary>
        /// Representa se é um fucionario ativo.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Função do funcionario.
        /// </summary>
        public string Funcao { get; set; }

        /// <summary>
        /// Hora de inicio do expediente.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de retorno do almoço.
        /// </summary>
        public TimeSpan HoraRetornoAlmoco { get; set; }

        /// <summary>
        /// Hora de saida para o almoço.
        /// </summary>
        public TimeSpan HoraSaidaAlmoco { get; set; }

        /// <summary>
        /// Hora de termino do expediente.
        /// </summary>
        public TimeSpan HoraTermino { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nome do Funcionario.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Numero de registro.
        /// </summary>
        public int Registro { get; set; }
    }
}
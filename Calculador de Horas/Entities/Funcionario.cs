using Calculador_de_Horas.Database;
using System;
using System.Collections.Generic;

namespace Calculador_de_Horas.Entities
{
    /// <summary>
    /// Classe para registro de funcionario.
    /// </summary>
    public class Funcionario
    {
        /// <summary>
        /// Id gerado pelo BD.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Numero de registro.
        /// </summary>
        public int Registro { get; set; }

        /// <summary>
        /// Nome do Funcionario.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Função do funcionario.
        /// </summary>
        public string Funcao { get; set; }

        /// <summary>
        /// Hora de inicio do expediente.
        /// </summary>
        public TimeSpan HoraIncio { get; set; }

        /// <summary>
        /// Hora de termino do expediente.
        /// </summary>
        public TimeSpan HoraTermino { get; set; }

        /// <summary>
        /// Hora de saida para o almoço.
        /// </summary>
        public TimeSpan HoraAlmocoSaida { get; set; }

        /// <summary>
        /// Hora de retorno do almoço.
        /// </summary>
        public TimeSpan HoraAlmocoRetorno { get; set; }

        /// <summary>
        /// Lista contendo os registros do banco de horas.
        /// </summary>
        public List<BancoDeHoras> BancoDeHoras { get; set; } = new List<BancoDeHoras>();

        /// <summary>
        /// Lista contendo os registros de horas extras.
        /// </summary>
        public List<HorasFuncionario> CartaoPonto { get; set; } = new List<HorasFuncionario>();

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public Funcionario()
        {
        }

        /// <summary>
        /// Contrutor com adição de funcionario.
        /// </summary>
        /// <param name="registro">Numero de registro.</param>
        /// <param name="nome">Nome</param>
        /// <param name="funcao">Função</param>
        /// <param name="horaIncio">Hora de inicio do expediente.</param>
        /// <param name="horaTermino">Hora de termino do expediente.</param>
        /// <param name="horaAlmocoSaida">Hora de saida para almoço</param>
        /// <param name="horaAlmocoRetorno">Hora de retorno do almoço</param>
        public Funcionario(int registro, string nome, string funcao, TimeSpan horaIncio, TimeSpan horaTermino, TimeSpan horaAlmocoSaida, TimeSpan horaAlmocoRetorno)
        {
            Registro = registro;
            Nome = nome;
            Funcao = funcao;
            HoraIncio = horaIncio;
            HoraTermino = horaTermino;
            HoraAlmocoSaida = horaAlmocoSaida;
            HoraAlmocoRetorno = horaAlmocoRetorno;
        }

        /// <summary>
        /// Metodo para adição de horas.
        /// </summary>
        /// <param name="horasFuncionario">Objeto do tipo HorasFuncionario para Adição.</param>
        public void AddMarcacaoPonto(HorasFuncionario horasFuncionario)
        {
            CartaoPonto.Add(horasFuncionario);

            if (horasFuncionario.Extras.Minutes != 0 || horasFuncionario.Extras.Hours != 0)
            {
                BancoDeHoras bancoDeHoras = new BancoDeHoras(horasFuncionario.Extras, "Horas Extras", horasFuncionario.DataRegistro);
                AtualizarBancoHoras(bancoDeHoras);
            }
        }

        /// <summary>
        /// Metodo para adição de horas no banco de horas.
        /// </summary>
        /// <param name="bancoHoras">Objeto do tipo BancoDeHoras a ser atualizado.</param>
        public void AtualizarBancoHoras(BancoDeHoras bancoHoras)
        {
            BancoDeHoras.Add(bancoHoras);
        }

        public Funcionario BuscarFuncionario(int numeroRegistro, MyDatabaseContext myDatabase)
        {
            Funcionario result = myDatabase.BuscarFuncionario(numeroRegistro);
            return result;
        }

        public List<Funcionario> BuscarFuncionarios(MyDatabaseContext myDatabase)
        {
            List<Funcionario> todosFuncionarios = myDatabase.BuscarFuncionario();

            return todosFuncionarios;
        }

        public void SalvarFuncionario(MyDatabaseContext myDatabase)
        {
            myDatabase.SalvarFuncionario(this);
        }
    }
}
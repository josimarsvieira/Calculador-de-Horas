using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections;
using System.Windows;

namespace Calculador_de_Horas
{
    /// <summary>
    /// Interaction logic for BancoDeHoras.xaml
    /// </summary>
    public partial class BancoDeHorasWindow : Window
    {
        /// <summary>
        /// Janela para exibição do banco de horas do funcionario ativo.
        /// </summary>
        /// <param name="registro">Numero do registro do funcionario a ser recuperado.</param>
        /// <param name="mes">Mes referente ao periodo a ser exibido.</param>
        /// <param name="ano">Ano referente ao periodo a ser exibido.</param>
        public BancoDeHorasWindow(int registro, DateTime dataBusca)
        {
            InitializeComponent();
            RefreshWindow(registro, dataBusca);

        }

        /// <summary>
        /// Metodo para atualização dos campos da janela.
        /// </summary>
        /// <param name="registro">Numero do registro do funcionario a ser recuperado.</param>
        /// <param name="mes">Mes referente ao periodo a ser exibido.</param>
        /// <param name="ano">Ano referente ao periodo a ser exibido.</param>
        public void RefreshWindow(int registro, DateTime dataBusca)
        {
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario funcionario;

                try
                {
                    funcionario = dbContext.BuscarFuncionario(registro);
                    IEnumerable bancoDeHoras = dbContext.BuscaBancoDeHorasFiltrado(funcionario, dataBusca);
                    foreach (BancoDeHoras h in bancoDeHoras)
                    {
                        listBoxRegistros.Items.Add(
                            $"{h.DataRegistro.ToShortDateString()} \t" +
                            $"{h.HorasExtras.ToString()}\t\t" +
                            $"{h.Justificativa}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

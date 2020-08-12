using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Interaction logic for BancoDeHoras.xaml
    /// </summary>
    public partial class BancoDeHorasWindow : Window
    {
        /// <summary>
        /// Janela para exibição do banco de horas do funcionario ativo.
        /// </summary>
        /// <param name="id">Id do funcionario a ser recuperado.</param>
        /// <param name="dataBusca">Mes referente ao periodo a ser exibido.</param>
        public BancoDeHorasWindow(string id, DateTime dataBusca)
        {
            InitializeComponent();
            RefreshWindow(id, dataBusca).ConfigureAwait(true);
        }

        /// <summary>
        /// Metodo para atualização dos campos da janela.
        /// </summary>
        /// <param name="id">Id do funcionario a ser recuperado.</param>
        /// <param name="dataBusca">Mes referente ao periodo a ser exibido.</param>
        public async Task RefreshWindow(string id, DateTime dataBusca)
        {
            Funcionario funcionario;

            try
            {
                funcionario = await ClientApi.GetEmployeeAsync(id).ConfigureAwait(true);
                List<BancoDeHoras> bancoDeHoras = await ClientApi.GetBankHoursFilteredAsync(funcionario, dataBusca).ConfigureAwait(true);

                dgBanco.ItemsSource = bancoDeHoras;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
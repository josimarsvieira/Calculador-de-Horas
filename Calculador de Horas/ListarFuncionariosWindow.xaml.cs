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
    /// Lógica interna para ListarFuncionariosWindow.xaml
    /// </summary>
    public partial class ListarFuncionariosWindow : Window
    {
        private List<Funcionario> funcionarios;

        private int selecao = 0;

        /// <summary>
        /// Construtor da janela de exibição dos funcionários.
        /// </summary>
        public ListarFuncionariosWindow()
        {
            InitializeComponent();
            PreencheListBox().ConfigureAwait(true);
        }

        /// <summary>
        /// Objeto contendo o funcionário selecionado.
        /// </summary>
        public Funcionario FuncionarioAtual { get; private set; }

        /// <summary>
        /// Preenche a lista de funcionarios
        /// </summary>
        public async Task PreencheListBox()
        {
            try
            {
                switch (selecao)
                {
                    case 0:
                        funcionarios = await ClientApi.GetEmployeesAsync(true).ConfigureAwait(true);
                        break;

                    case 1:
                        funcionarios = await ClientApi.GetEmployeesAsync(false).ConfigureAwait(true);
                        break;

                    case 2:
                        funcionarios = await ClientApi.GetEmployeesAsync().ConfigureAwait(true);
                        break;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }

            dgFuncionarios.ItemsSource = funcionarios;
        }

        /// <summary>
        /// Evento click do botão editar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarFuncionario();

            if (FuncionarioAtual != null)
            {
                GestaoDeFuncionarioWindow windows = new GestaoDeFuncionarioWindow(FuncionarioAtual);
                windows.ShowDialog();
                PreencheListBox().ConfigureAwait(true);
            }
        }

        private void BtnMostrar_Click(object sender, RoutedEventArgs e)
        {
            switch (selecao)
            {
                case 0:
                    btnMostrar.Content = "Todos";
                    selecao = 1;
                    break;

                case 1:
                    btnMostrar.Content = "Ativos";
                    selecao = 2;
                    break;

                default:
                    btnMostrar.Content = "Inativos";
                    selecao = 0;
                    break;
            }

            PreencheListBox().ConfigureAwait(true);
        }

        /// <summary>
        /// Evento click do botão novo funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            GestaoDeFuncionarioWindow windows = new GestaoDeFuncionarioWindow(null);
            windows.ShowDialog();
            PreencheListBox().ConfigureAwait(true);
        }

        /// <summary>
        /// Evento click do botão selecionar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarFuncionario();

            if (FuncionarioAtual == null)
            {
                return;
            }

            Close();
        }

        private void SelecionarFuncionario()
        {
            if (dgFuncionarios.SelectedItem != null)
            {
                FuncionarioAtual = (Funcionario)dgFuncionarios.SelectedItem;
            }
            else
            {
                MessageBox.Show("Nenhum funcionário selecionado!");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
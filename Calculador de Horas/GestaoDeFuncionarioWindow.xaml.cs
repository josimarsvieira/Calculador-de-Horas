using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System;
using System.Globalization;
using System.Net.Http;
using System.Windows;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Interaction logic for GestaoDeFuncionarioWindow.xaml
    /// </summary>
    public partial class GestaoDeFuncionarioWindow : Window
    {
        /// <summary>
        /// Janela para adição e edição de funcionarios
        /// </summary>
        public GestaoDeFuncionarioWindow(Funcionario funcionario)
        {
            InitializeComponent();

            if (funcionario != null)
            {
                txtRegistro.Text = funcionario.Registro.ToString(CultureInfo.InvariantCulture);
                txtRegistro.IsEnabled = false;
                PreencherCampos(funcionario);
            }
        }

        /// <summary>
        /// Evento click do botão salvar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionario;

            Funcionario busca = await ClientApi.GetEmployeeAsync(int.Parse(txtRegistro.Text, CultureInfo.InvariantCulture)).ConfigureAwait(true);

            try
            {
                funcionario = new Funcionario(int.Parse(txtRegistro.Text, CultureInfo.InvariantCulture), txtNome.Text, txtFuncao.Text,
                    new TimeSpan(timePickerEntrada.SelectedTime.Value.Hour, timePickerEntrada.SelectedTime.Value.Minute, 0),
                    new TimeSpan(timePickerSaida.SelectedTime.Value.Hour, timePickerSaida.SelectedTime.Value.Minute, 0),
                    new TimeSpan(timePickerSaidaAlmoco.SelectedTime.Value.Hour, timePickerSaidaAlmoco.SelectedTime.Value.Minute, 0),
                    new TimeSpan(timePickerRetornoAlmoco.SelectedTime.Value.Hour, timePickerRetornoAlmoco.SelectedTime.Value.Minute, 0),
                    checkAtivo.IsChecked.Value);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Favor conferir os dados informados " + ex.Message);
                return;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Existem campos em branco");
                return;
            }

            if (busca != null)
            {
                switch (MessageBox.Show("Cadastro já existente!\nDeseja Atualizar com esses dados?", "Confirmar alteração", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            busca.HoraInicio = funcionario.HoraInicio;
                            busca.HoraSaidaAlmoco = funcionario.HoraSaidaAlmoco;
                            busca.HoraRetornoAlmoco = funcionario.HoraRetornoAlmoco;
                            busca.HoraTermino = funcionario.HoraTermino;
                            busca.Ativo = funcionario.Ativo;

                            await ClientApi.UpdateEmployeeAsync(busca).ConfigureAwait(true);
                            MessageBox.Show("Cadastro efetuado com sucesso.");
                            Close();
                        }
                        catch (HttpRequestException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        break;

                    case MessageBoxResult.No:
                        PreencherCampos(busca);
                        break;

                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            else
            {
                try
                {
                    await ClientApi.CreateEmployeeAsync(funcionario).ConfigureAwait(true);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                MessageBox.Show("Cadastro efetuado com sucesso.");
                Close();
            }
        }

        /// <summary>
        /// Metodo para preenchimento dos campos com os dados do funcionario recuperado do BD.
        /// </summary>
        /// <param name="busca">Objeto Funcionario recuperado do BD.</param>
        private void PreencherCampos(Funcionario busca)
        {
            if (busca != null)
            {
                txtNome.Text = busca.Nome;
                txtFuncao.Text = busca.Funcao;
                timePickerEntrada.SelectedTime = new DateTime().Add(busca.HoraInicio);
                timePickerSaidaAlmoco.SelectedTime = new DateTime().Add(busca.HoraSaidaAlmoco);
                timePickerRetornoAlmoco.SelectedTime = new DateTime().Add(busca.HoraRetornoAlmoco);
                timePickerSaida.SelectedTime = new DateTime().Add(busca.HoraTermino);
                checkAtivo.IsChecked = busca.Ativo;
                btnSalvar.Content = "Atualizar";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
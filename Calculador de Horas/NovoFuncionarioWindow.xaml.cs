using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Calculador_de_Horas
{
    /// <summary>
    /// Interaction logic for NovoFuncionarioWindow.xaml
    /// </summary>
    public partial class NovoFuncionarioWindow : Window
    {
        /// <summary>
        /// Janela para adição e edição de funcionarios
        /// </summary>
        public NovoFuncionarioWindow(string registro)
        {
            InitializeComponent();
            txtRegistro.Text = registro;
            PreencheComboBox();

            if (registro != "")
            {
                txtRegistro.IsEnabled = false;
                PreencherCampos(BuscaFuncionario(registro));
            }
        }


        private Funcionario BuscaFuncionario(string registro)
        {
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario busca = dbContext.BuscarFuncionario(int.Parse(txtRegistro.Text));

                if (busca != null)
                {
                    return busca;
                }

                return null;
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
                cbHoraIncio.Text = busca.HoraIncio.Hours < 10 ? ("0" + busca.HoraIncio.Hours.ToString()) : busca.HoraIncio.Hours.ToString();
                cbMinutosInicio.Text = busca.HoraIncio.Minutes < 10 ? ("0" + busca.HoraIncio.Minutes.ToString()) : busca.HoraIncio.Minutes.ToString(); ;
                cbHoraTermino.Text = busca.HoraTermino.Hours < 10 ? ("0" + busca.HoraTermino.Hours.ToString()) : busca.HoraTermino.Hours.ToString(); ;
                cbMinutosTermino.Text = busca.HoraTermino.Minutes < 10 ? ("0" + busca.HoraTermino.Minutes.ToString()) : busca.HoraTermino.Minutes.ToString(); ;
                cbHoraAlmocoSaida.Text = busca.HoraAlmocoSaida.Hours < 10 ? ("0" + busca.HoraAlmocoSaida.Hours.ToString()) : busca.HoraAlmocoSaida.Hours.ToString(); ;
                cbMinutosAlmocoSaida.Text = busca.HoraAlmocoSaida.Minutes < 10 ? ("0" + busca.HoraAlmocoSaida.Minutes.ToString()) : busca.HoraAlmocoSaida.Minutes.ToString(); ;
                cbHoraAlmocoRetorno.Text = busca.HoraAlmocoRetorno.Hours < 10 ? ("0" + busca.HoraAlmocoRetorno.Hours.ToString()) : busca.HoraAlmocoRetorno.Hours.ToString(); ;
                cbMinutosAlmocoRetorno.Text = busca.HoraAlmocoRetorno.Minutes < 10 ? ("0" + busca.HoraAlmocoRetorno.Minutes.ToString()) : busca.HoraAlmocoRetorno.Minutes.ToString(); ;
                btnSalvar.Content = "Atualizar";
            }
        }

        /// <summary>
        /// Evento click do botão salvar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Funcionario funcionario;
            try
            {
                funcionario = new Funcionario(int.Parse(txtRegistro.Text), txtNome.Text, txtFuncao.Text,
                    new TimeSpan(int.Parse(cbHoraIncio.SelectionBoxItem.ToString()), int.Parse(cbMinutosInicio.SelectionBoxItem.ToString()), 0),
                    new TimeSpan(int.Parse(cbHoraTermino.SelectionBoxItem.ToString()), int.Parse(cbMinutosTermino.SelectionBoxItem.ToString()), 0),
                    new TimeSpan(int.Parse(cbHoraAlmocoSaida.SelectionBoxItem.ToString()), int.Parse(cbMinutosAlmocoSaida.SelectionBoxItem.ToString()), 0),
                    new TimeSpan(int.Parse(cbHoraAlmocoRetorno.SelectionBoxItem.ToString()), int.Parse(cbMinutosAlmocoRetorno.SelectionBoxItem.ToString()), 0));
            }
            catch (FormatException)
            {
                MessageBox.Show("Favor conferir os dados informados");
                return;
            }

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario busca = dbContext.BuscarFuncionario(int.Parse(txtRegistro.Text));

                if (busca != null)
                {
                    switch (MessageBox.Show("Cadastro já existente!\nDeseja Atualizar com esses dados?", "Confirmar alteração", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                    {
                        case MessageBoxResult.Yes:
                            try
                            {
                                dbContext.AtualizarFuncionario(funcionario, busca);
                                MessageBox.Show("Cadastro efetuado com sucesso.");
                                TranferenciaDados.Registro = int.Parse(txtRegistro.Text);
                                Close();
                            }
                            catch (Exception ex)
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
                        dbContext.SalvarFuncionario(funcionario);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao gravar os dados, contate o suporte\nPara uso do TI. Menssagem: " + ex.Message);
                    }
                    MessageBox.Show("Cadastro efetuado com sucesso.");
                    TranferenciaDados.Registro = int.Parse(txtRegistro.Text);
                    Close();
                }
            }
        }

        /// <summary>
        /// Popula os comboBox da interface.
        /// </summary>
        private void PreencheComboBox()
        {
            List<string> Horas = new List<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            List<string> Minutos = new List<string> { "00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29",
                "30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47","48","49","50","51","52","53","54","55","56","57","58","59"};

            cbHoraIncio.ItemsSource = Horas;
            cbMinutosInicio.ItemsSource = Minutos;
            cbHoraTermino.ItemsSource = Horas;
            cbMinutosTermino.ItemsSource = Minutos;
            cbHoraAlmocoSaida.ItemsSource = Horas;
            cbMinutosAlmocoSaida.ItemsSource = Minutos;
            cbHoraAlmocoRetorno.ItemsSource = Horas;
            cbMinutosAlmocoRetorno.ItemsSource = Minutos;
        }
    }
}

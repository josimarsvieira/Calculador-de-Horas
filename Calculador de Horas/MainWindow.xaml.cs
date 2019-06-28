using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Calculador_de_Horas
{
    /// <summary>
    /// Interface Principal
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Contrutor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            VerificaCadastroEmpresa();
            PreencheComboBox();
        }

        /* ************************** Metodos responsaveis pela exibição da tela ************************** */

        /// <summary>
        /// Atualiza os campos da janela.
        /// </summary>
        /// <param name="func">Objeto do tipo Funcionario recuperado do BD.</param>
        private List<DateTime> RefreshList(Funcionario func)
        {
            txtBlockDias.Text = "";
            txtBlockEntrada.Text = "";
            txtBlockSaida.Text = "";
            txtBlockExtras.Text = "";
            lblBancoHoras.Content = "";

            TimeSpan totalHoras = new TimeSpan();
            TimeSpan totalBancoHoras = new TimeSpan();

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {

                IEnumerable horas;
                List<DateTime> datasComRegistros = new List<DateTime>();

                horas = dbContext.BuscaCartaoPonto(func, new DateTime(DPdata.SelectedDate.Value.Year, DPdata.SelectedDate.Value.Month, TranferenciaDados.Empresa.DiaFechamento - 1),
                    new DateTime(DPdata.SelectedDate.Value.Month == 1 ? DPdata.SelectedDate.Value.Year - 1 : DPdata.SelectedDate.Value.Year, DPdata.SelectedDate.Value.Month == 1 ? 12 : DPdata.SelectedDate.Value.Month - 1, TranferenciaDados.Empresa.DiaFechamento));

                IEnumerable bancoDeHoras = dbContext.BuscaBancoDeHoras(func);

                StringBuilder dias = new StringBuilder();
                StringBuilder entradas = new StringBuilder();
                StringBuilder saidas = new StringBuilder();
                StringBuilder extras = new StringBuilder();

                foreach (HorasFuncionario h in horas)
                {
                    dias.AppendLine(h.DataRegistro.Day < 10 ? $"0{h.DataRegistro.Day}" : $"{h.DataRegistro.Day}");
                    datasComRegistros.Add(h.DataRegistro);
                    entradas.AppendLine($"{h.Entrada.ToString()}");
                    saidas.AppendLine($"{h.Saida.ToString()}");
                    extras.AppendLine($"{h.Extras.ToString()}");
                    totalHoras += h.Extras;
                    TranferenciaDados.UltimaHoraAdicionada = h;
                }

                txtBlockDias.Text = dias.ToString();
                txtBlockEntrada.Text = entradas.ToString();
                txtBlockSaida.Text = saidas.ToString();
                txtBlockExtras.Text = extras.ToString();

                foreach (BancoDeHoras h in bancoDeHoras)
                {
                    totalBancoHoras += h.HorasExtras;
                }

                TimeSpan totalHoraAlmoco = func.HoraAlmocoRetorno.Subtract(func.HoraAlmocoSaida);
                TimeSpan horaTrabalho = func.HoraTermino.Subtract(func.HoraIncio).Subtract(totalHoraAlmoco);
                TimeSpan totalHorasFolga = new TimeSpan();

                lblBancoHoras.Content = totalBancoHoras;
                lblTotalExtras.Content = totalHoras.ToString();



                if (totalBancoHoras >= horaTrabalho)
                {
                    while (totalBancoHoras >= horaTrabalho)
                    {
                        totalHorasFolga = totalHorasFolga.Add(new TimeSpan(1, 0, 0, 0));
                        totalBancoHoras = totalBancoHoras.Subtract(horaTrabalho);
                    }
                }
                else if (totalBancoHoras <= -horaTrabalho)
                {
                    while (totalBancoHoras <= -horaTrabalho)
                    {
                        totalHorasFolga = totalHorasFolga.Subtract(new TimeSpan(1, 0, 0, 0));
                        totalBancoHoras = totalBancoHoras.Add(horaTrabalho);
                    }
                }

                lblHoraFolga.Content = $"{totalHorasFolga.Days} Dia(s) e {totalBancoHoras.Hours} Horas";
                if (totalHorasFolga.Days >= 0)
                {
                    lblHoraFolga.Foreground = Brushes.Black;
                }
                else
                {
                    lblHoraFolga.Foreground = Brushes.Red;
                }
                return datasComRegistros;
            }

        }

        /// <summary>
        /// Popula as comboBox da Janela.
        /// </summary>
        private void PreencheComboBox()
        {
            List<string> Horas = new List<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            List<string> HorasE = new List<string> { "-23", "-22", "-21", "-20", "-19", "-18", "-17", "-16", "-15", "-14", "-13", "-12", "-11", "-10", "-09", "-08", "-07", "-06", "-05", "-04", "-03", "-02","-01",
                "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"};
            List<string> Minutos = new List<string> { "00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29",
                "30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47","48","49","50","51","52","53", "54","55","56","57","58","59"};

            cbHoraEntrada.ItemsSource = Horas;
            cbMinutosEntrada.ItemsSource = Minutos;
            cbHoraSaida.ItemsSource = Horas;
            cbMinutosSaida.ItemsSource = Minutos;
            cbHoraExtras.ItemsSource = HorasE;
            cbMinutosExtras.ItemsSource = Minutos;
        }

        /* ************************** Metodos de manipulação de dados ************************** */

        /// <summary>
        /// Verifica se existe empresa cadastrada
        /// </summary>
        private void VerificaCadastroEmpresa()
        {
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Empresa empresa;

                try
                {
                    empresa = dbContext.BuscarEmpresa();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (empresa != null)
                {
                    JanelaPrincipal.Title = $"Calculador de Horas - {empresa.RazaoSocial}";
                    TranferenciaDados.Empresa = empresa;
                }
                else
                {
                    NovaEmpresaWindow windows;
                    bool decisao = false;

                    do
                    {
                        windows = new NovaEmpresaWindow();

                        windows.ShowDialog();

                        if (windows.Empresa == null)
                        {
                            if (MessageBox.Show("Empresa não cadastrada, deseja voltar e completar o cadastro?", "Empresa não Cadastrada", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                decisao = true;
                            }
                            else
                            {
                                decisao = false;
                                Close();
                            }
                        }
                        else
                        {
                            JanelaPrincipal.Title = $"Calculador de Horas - {windows.Empresa.RazaoSocial}";
                            TranferenciaDados.Empresa = windows.Empresa;
                        }
                    } while (decisao);
                }
            }
        }

        /// <summary>
        /// Recupera um funcionario do BD.
        /// </summary>
        private void BuscarFuncionario()
        {
            try
            {
                if (txtRegistro.Text != "")
                {
                    TranferenciaDados.Registro = int.Parse(txtRegistro.Text);
                }
                else
                {
                    ListarFuncionariosWindow windows = new ListarFuncionariosWindow();
                    windows.ShowDialog();
                    txtRegistro.Text = TranferenciaDados.Registro.ToString();
                    if (txtRegistro.Text == "")
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario funcionario;

                try
                {
                    funcionario = dbContext.BuscarFuncionario(int.Parse(txtRegistro.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (funcionario != null)
                {
                    txtNome.Content = funcionario.Nome;
                    btnBancoHoras.IsEnabled = true;
                    RefreshList(funcionario);
                    TranferenciaDados.Funcionario = funcionario;
                }
                else
                {
                    GestaoDeFuncionarioWindow windows = new GestaoDeFuncionarioWindow(txtRegistro.Text);
                    windows.ShowDialog();
                    txtRegistro.Text = TranferenciaDados.Registro.ToString();
                    btnBancoHoras.IsEnabled = false;
                }
            }

        }

        /// <summary>
        /// Metodo para editar hora
        /// </summary>
        /// <param name="dataBtnAdicionar">Data a ser editada</param>
        private void EditarHora(DateTime dataBtnAdicionar)
        {
            EditarHorasWindow editarUltimo = new EditarHorasWindow(dataBtnAdicionar);
            editarUltimo.ShowDialog();
            RefreshList(TranferenciaDados.Funcionario);
        }

        /* ************************** Eventos de Click em botões ************************** */

        /// <summary>
        /// Evento click do botão adicionar, esse é o principal botão da interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            DateTime dataSelecionada = DPdata.SelectedDate.Value;

            try
            {
                using (MyDatabaseContext dbContext = new MyDatabaseContext())
                {
                    Funcionario funcionario = dbContext.BuscarFuncionario(int.Parse(txtRegistro.Text));
                    List<DateTime> cartaoPonto = RefreshList(funcionario);

                    if (cbMinutosExtras.SelectedIndex != 0 || cbHoraExtras.SelectedIndex != 23)
                    {
                        if (MessageBox.Show("Desaja incluir hora extra munualmente?", "Confirmar Inclusão de Horas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            int hora = int.Parse(cbHoraExtras.SelectionBoxItem.ToString());
                            int minutos = hora < 0 ? -int.Parse(cbMinutosExtras.SelectionBoxItem.ToString()) : int.Parse(cbMinutosExtras.SelectionBoxItem.ToString());

                            TimeSpan HorasExtras = new TimeSpan(hora, minutos, 0);
                            funcionario.AtualizarBancoHoras(new BancoDeHoras(HorasExtras, "Hora Adicionada Manualmente", dataSelecionada));
                            dbContext.UpdateBanco(funcionario);

                            RefreshList(funcionario);
                        }

                        cbHoraExtras.SelectedIndex = 23;
                        cbMinutosExtras.SelectedIndex = 0;
                        return;
                    }

                    TimeSpan entrada = new TimeSpan(int.Parse(cbHoraEntrada.SelectionBoxItem.ToString()), int.Parse(cbMinutosEntrada.SelectionBoxItem.ToString()), 0);
                    TimeSpan saida = new TimeSpan(int.Parse(cbHoraSaida.SelectionBoxItem.ToString()), int.Parse(cbMinutosSaida.SelectionBoxItem.ToString()), 0);
                    TimeSpan totalHoraAlmoco = funcionario.HoraAlmocoRetorno.Subtract(funcionario.HoraAlmocoSaida);
                    TimeSpan horaTrabalho = funcionario.HoraTermino.Subtract(funcionario.HoraIncio).Subtract(totalHoraAlmoco);

                    HorasFuncionario horas = new HorasFuncionario(entrada, saida, dataSelecionada);

                    if (cartaoPonto.Contains(horas.DataRegistro))
                    {
                        if (MessageBox.Show("Já existe registro com essa data, deseja alterar?", "Hora Existente", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            EditarHora(dataSelecionada);
                        }
                    }
                    else if (dataSelecionada.DayOfWeek.ToString().ToUpper().Equals("SATURDAY") || dataSelecionada.DayOfWeek.ToString().ToUpper().Equals("SUNDAY"))
                    {
                        if (MessageBox.Show($"Proximo registro é { (dataSelecionada.DayOfWeek.ToString() == "Saturday" ? "Sabado" : "Domingo") } confirma a inclusão?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            horas = new HorasFuncionario(entrada, saida, dataSelecionada);
                            horas.CalculaExtras(new TimeSpan(0, 0, 0));//Horario de trabalho em branco pois não há expediente normal.
                            funcionario.AddMarcacaoPonto(horas);
                        }
                        else
                        {
                            horas = new HorasFuncionario(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), dataSelecionada);
                            horas.CalculaExtras(new TimeSpan(0, 0, 0));
                            funcionario.AddMarcacaoPonto(horas);
                        }

                    }

                    else
                    {
                        if (cbHoraEntrada.SelectedIndex == 0 && cbMinutosEntrada.SelectedIndex == 0 && cbHoraSaida.SelectedIndex == 0 && cbMinutosEntrada.SelectedIndex == 0)
                        {
                            switch (MessageBox.Show("Não foi informado hora de entrada e saida para esse funcionario.\nSelecione Sim para marcar como falta total.\nSelecione Não para falta justificada.", "Verificar Falta", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                            {
                                case MessageBoxResult.Yes:
                                    horas.CalculaExtras(horaTrabalho);
                                    break;
                                case MessageBoxResult.No:
                                    TimeSpan extras = horas.CalculaExtras(new TimeSpan(0, 0, 0));
                                    funcionario.AtualizarBancoHoras(new BancoDeHoras(extras, $"Falta abonada em {DateTime.Now.ToShortDateString()}", dataSelecionada));
                                    dbContext.UpdateBanco(funcionario);
                                    break;
                                case MessageBoxResult.Cancel:
                                    return;
                            }
                        }
                        else if (cbHoraEntrada.SelectedIndex == 0 && cbMinutosEntrada.SelectedIndex == 0 || cbHoraEntrada.SelectedIndex >= 12)
                        {
                            if (MessageBox.Show("Não foi informado hora de entrada para esse funcionario.\nDeseja marcar como falta parcial?", "Verificar Falta", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                horas.Entrada = funcionario.HoraAlmocoRetorno;
                                horas.CalculaExtras(horaTrabalho);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else if (cbHoraSaida.SelectedIndex == 0 && cbMinutosSaida.SelectedIndex == 0 || cbHoraSaida.SelectedIndex <= 12)
                        {
                            if (MessageBox.Show("Não foi informado hora de saida para esse funcionario\nDeseja marcar como falta parcial?", "Verificar Falta", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                horas.Saida = funcionario.HoraAlmocoSaida;
                                horas.CalculaExtras(horaTrabalho);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            horas.CalculaExtras(horaTrabalho.Add(totalHoraAlmoco));

                            if (horas.Extras <= new TimeSpan(0, 10, 0) && horas.Extras >= new TimeSpan(0, -10, 0))
                            {
                                horas.Extras = new TimeSpan(0, 0, 0);
                            }
                        }

                        funcionario.AddMarcacaoPonto(horas);
                    }

                    dbContext.UpdateFuncionario(funcionario);
                    RefreshList(funcionario);
                    DPdata.SelectedDate = dataSelecionada.AddDays(1);
                }
            }
            catch (FormatException)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Evento click do botão atualizar campos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            BuscarFuncionario();
        }

        /// <summary>
        /// Evento click do botão detalhar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetalhar_Click(object sender, RoutedEventArgs e)
        {
            GestaoDeFuncionarioWindow windows = new GestaoDeFuncionarioWindow(txtRegistro.Text);
            windows.ShowDialog();
            BuscarFuncionario();

        }

        /// <summary>
        /// Evento click do botão banco de horas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBancoHoras_Click(object sender, RoutedEventArgs e)
        {
            DateTime dataBusca = new DateTime(DPdata.SelectedDate.Value.Year, DPdata.SelectedDate.Value.Month, TranferenciaDados.Empresa.DiaFechamento);
            BancoDeHorasWindow bancoDeHorasWindow = new BancoDeHorasWindow(int.Parse(txtRegistro.Text.ToString()), dataBusca);
            bancoDeHorasWindow.ShowDialog();
        }

        /// <summary>
        /// Evento click botão Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            txtRegistro.Text = "";
            BuscarFuncionario();
        }

        /* ************************** Eventos de troca nas comboBox ************************** */

        /// <summary>
        /// Quando troca o valor do campo Hora Entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbHoraEntrada_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbHoraEntrada.SelectedIndex != 0)
            {
                cbHoraExtras.IsEnabled = false;
                cbMinutosExtras.IsEnabled = false;
                cbHoraExtras.SelectedIndex = 23;
                cbMinutosExtras.SelectedIndex = 0;
            }
            else if(cbMinutosSaida.SelectedIndex == 0 && cbHoraSaida.SelectedIndex == 0 && cbMinutosEntrada.SelectedIndex == 0)
            {
                cbHoraExtras.IsEnabled = true;
                cbMinutosExtras.IsEnabled = true;
            }
        }

        /// <summary>
        /// Quando troca o valor do campo Minuto Entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbMinutosEntrada_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbMinutosEntrada.SelectedIndex != 0)
            {
                cbHoraExtras.IsEnabled = false;
                cbMinutosExtras.IsEnabled = false;
                cbHoraExtras.SelectedIndex = 23;
                cbMinutosExtras.SelectedIndex = 0;
            }
            else if(cbMinutosSaida.SelectedIndex == 0 && cbHoraSaida.SelectedIndex == 0 && cbHoraEntrada.SelectedIndex == 0)
            {
                cbHoraExtras.IsEnabled = true;
                cbMinutosExtras.IsEnabled = true;
            }
        }

        /// <summary>
        /// Quando troca o valor do campo Hora Saida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbHoraSaida_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbHoraSaida.SelectedIndex != 0)
            {
                cbHoraExtras.IsEnabled = false;
                cbMinutosExtras.IsEnabled = false;
                cbHoraExtras.SelectedIndex = 23;
                cbMinutosExtras.SelectedIndex = 0;
            }
            else if (cbMinutosEntrada.SelectedIndex == 0 && cbHoraEntrada.SelectedIndex == 0 && cbMinutosSaida.SelectedIndex == 0)
            {
                cbHoraExtras.IsEnabled = true;
                cbMinutosExtras.IsEnabled = true;
            }
        }

        /// <summary>
        /// Quando troca o valor do campo Minuto Saida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbMinutosSaida_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbMinutosSaida.SelectedIndex != 0)
            {
                cbHoraExtras.IsEnabled = false;
                cbMinutosExtras.IsEnabled = false;
                cbHoraExtras.SelectedIndex = 23;
                cbMinutosExtras.SelectedIndex = 0;
            }
            else if (cbMinutosEntrada.SelectedIndex == 0 && cbHoraSaida.SelectedIndex == 0 && cbHoraEntrada.SelectedIndex == 0)
            {
                cbHoraExtras.IsEnabled = true;
                cbMinutosExtras.IsEnabled = true;
            }
        }
    }
}

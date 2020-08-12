using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Interface Principal
    /// </summary>
    public partial class MainWindow : Window
    {
        private Empresa Empre;
        private Funcionario Func;

        /// <summary>
        /// Contrutor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            VerificaCadastroEmpresa().ConfigureAwait(true);
            DPdata.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Evento click do botão adicionar, esse é o principal botão da interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dataSelecionada = DPdata.SelectedDate.Value;

                await RefreshList(Func).ConfigureAwait(true);

                if (timePickerExtras.SelectedTime.HasValue)
                {
                    TimeSpan HorasExtras = new TimeSpan(timePickerExtras.SelectedTime.Value.Hour, timePickerExtras.SelectedTime.Value.Minute, 0);

                    switch (MessageBox.Show("Deseja incluir hora extra manualmente? \nSelecione Sim para INCLUIR \nSelecione Não para REMOVER", "Confirmar Inclusão de Horas", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                    {
                        case MessageBoxResult.Yes:
                            await ClientApi.CreateBankHoursAsync(new BancoDeHoras(HorasExtras, "Hora adicionada manualmente", dataSelecionada, Func.Registro)).ConfigureAwait(true);
                            break;

                        case MessageBoxResult.No:
                            HorasExtras = -HorasExtras;
                            await ClientApi.CreateBankHoursAsync(new BancoDeHoras(HorasExtras, "Hora removida manualmente", dataSelecionada, Func.Registro)).ConfigureAwait(true);
                            break;
                    }

                    await RefreshList(Func).ConfigureAwait(true);
                    return;
                }

                TimeSpan entrada;
                if (!timePickerEntrada.SelectedTime.HasValue)
                {
                    entrada = new TimeSpan(0, 0, 0);
                }
                else
                {
                    entrada = new TimeSpan(timePickerEntrada.SelectedTime.Value.Hour, timePickerEntrada.SelectedTime.Value.Minute, 0);
                }

                TimeSpan saida;
                if (!timePickerSaida.SelectedTime.HasValue)
                {
                    saida = new TimeSpan(0, 0, 0);
                }
                else
                {
                    saida = new TimeSpan(timePickerSaida.SelectedTime.Value.Hour, timePickerSaida.SelectedTime.Value.Minute, 0);
                }

                TimeSpan almocoSaida;
                if (!timePickerSaidaAlmoco.SelectedTime.HasValue)
                {
                    almocoSaida = Func.HoraSaidaAlmoco;
                }
                else
                {
                    almocoSaida = new TimeSpan(timePickerSaidaAlmoco.SelectedTime.Value.Hour, timePickerSaidaAlmoco.SelectedTime.Value.Minute, 0);
                }

                TimeSpan retornoAlmoco;
                if (!timePickerRetornoAlmoco.SelectedTime.HasValue)
                {
                    retornoAlmoco = Func.HoraRetornoAlmoco;
                }
                else
                {
                    retornoAlmoco = new TimeSpan(timePickerRetornoAlmoco.SelectedTime.Value.Hour, timePickerRetornoAlmoco.SelectedTime.Value.Minute, 0);
                }

                TimeSpan totalHoraAlmoco = Func.HoraRetornoAlmoco.Subtract(Func.HoraSaidaAlmoco);
                TimeSpan horaTrabalho = Func.HoraTermino.Subtract(Func.HoraInicio).Subtract(totalHoraAlmoco);

                HorasFuncionario horas = new HorasFuncionario(entrada, saida, almocoSaida, retornoAlmoco, Func.Registro, dataSelecionada);

                HorasFuncionario horaAlterar = null;

                if (Listhoras.ItemsSource != null)
                {
                    foreach (HorasFuncionario b1 in Listhoras.ItemsSource)
                    {
                        if (b1.DataRegistro.ToShortDateString() == dataSelecionada.ToShortDateString())
                        {
                            horaAlterar = b1;
                        }
                    }
                }

                if (horaAlterar != null)
                {
                    if (MessageBox.Show("Já existe registro com essa data, deseja alterar?", "Hora Existente", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        EditarHora(horaAlterar);
                        return;
                    }
                }
                else if (dataSelecionada.DayOfWeek.ToString().ToUpper(CultureInfo.InvariantCulture).Equals("SATURDAY", StringComparison.InvariantCulture) || dataSelecionada.DayOfWeek.ToString().ToUpper(CultureInfo.InvariantCulture).Equals("SUNDAY", StringComparison.InvariantCulture))
                {
                    if (MessageBox.Show($"Proximo registro é { (dataSelecionada.DayOfWeek.ToString() == "Saturday" ? "Sabado" : "Domingo") } confirma a inclusão?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (entrada == new TimeSpan(0, 0, 0))
                        {
                            almocoSaida = new TimeSpan(0, 0, 0);
                            retornoAlmoco = new TimeSpan(0, 0, 0);
                        }

                        horas = new HorasFuncionario(entrada, saida, almocoSaida, retornoAlmoco, Func.Registro, dataSelecionada);
                        horas.CalculaExtras(new TimeSpan(0, 0, 0)); //Horario de trabalho em branco pois não há expediente normal.
                        ConfirmacaoWindow confirmacaoWindow = new ConfirmacaoWindow(horas, Func);
                        confirmacaoWindow.ShowDialog();
                    }
                    else
                    {
                        horas = new HorasFuncionario(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), Func.Registro, dataSelecionada);
                        horas.CalculaExtras(new TimeSpan(0, 0, 0));
                        ConfirmacaoWindow confirmacaoWindow = new ConfirmacaoWindow(horas, Func);
                        confirmacaoWindow.ShowDialog();
                    }
                }
                else
                {
                    if (!timePickerEntrada.SelectedTime.HasValue && !timePickerSaida.SelectedTime.HasValue)
                    {
                        switch (MessageBox.Show("Não foi informado hora de entrada e saida para esse funcionario.\nSelecione Sim para marcar como falta total.\nSelecione Não para falta justificada.", "Verificar Falta", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                        {
                            case MessageBoxResult.Yes:
                                horas.HoraSaidaAlmoco = new TimeSpan(0, 0, 0);
                                horas.HoraRetornoAlmoco = new TimeSpan(0, 0, 0);

                                horas.CalculaExtras(horaTrabalho);
                                break;

                            case MessageBoxResult.No:
                                horas.HoraSaidaAlmoco = new TimeSpan(0, 0, 0);
                                horas.HoraRetornoAlmoco = new TimeSpan(0, 0, 0);

                                TimeSpan extras = horas.CalculaExtras(new TimeSpan(0, 0, 0));
                                await ClientApi.CreateBankHoursAsync(new BancoDeHoras(extras, $"Falta abonada em {DateTime.Now.ToShortDateString()}", dataSelecionada, Func.Registro)).ConfigureAwait(true);
                                break;

                            case MessageBoxResult.Cancel:
                                return;
                        }
                    }
                    else if (!timePickerEntrada.SelectedTime.HasValue)
                    {
                        if (MessageBox.Show("Não foi informado hora de entrada para esse funcionario.\nDeseja marcar como falta parcial?", "Verificar Falta", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (horas.HoraRetornoAlmoco != new TimeSpan(0, 0, 0))
                            {
                                horas.HoraSaidaAlmoco = new TimeSpan(0, 0, 0);
                                horas.CalculaExtras(horaTrabalho);
                            }
                            else
                            {
                                MessageBox.Show("Existe campos em branco!");
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (!timePickerSaida.SelectedTime.HasValue)
                    {
                        if (MessageBox.Show("Não foi informado hora de saida para esse funcionario\nDeseja marcar como falta parcial?", "Verificar Falta", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (horas.HoraSaidaAlmoco != new TimeSpan(0, 0, 0))
                            {
                                horas.HoraRetornoAlmoco = new TimeSpan(0, 0, 0);
                                horas.CalculaExtras(horaTrabalho);
                            }
                            else
                            {
                                MessageBox.Show("Existe campos em branco!");
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        horas.CalculaExtras(horaTrabalho);

                        if (horas.Extras <= new TimeSpan(0, 10, 0) && horas.Extras >= new TimeSpan(0, -10, 0))
                        {
                            horas.Extras = new TimeSpan(0, 0, 0);
                        }
                    }
                    ConfirmacaoWindow confirmacaoWindow = new ConfirmacaoWindow(horas, Func);
                    confirmacaoWindow.ShowDialog();
                }

                await RefreshList(Func).ConfigureAwait(true);
                DPdata.SelectedDate = dataSelecionada.AddDays(1);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAlterar_Click(object sender, RoutedEventArgs e)
        {
            EditarHora((HorasFuncionario)Listhoras.SelectedItem);
        }

        /// <summary>
        /// Evento click do botão atualizar campos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            BuscarFuncionario(false);
        }

        /// <summary>
        /// Evento click do botão banco de horas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBancoHoras_Click(object sender, RoutedEventArgs e)
        {
            DateTime dataBusca = new DateTime(DPdata.SelectedDate.Value.Year, DPdata.SelectedDate.Value.Month, Empre.DiaFechamento);
            BancoDeHorasWindow bancoDeHorasWindow = new BancoDeHorasWindow(Func.Id, dataBusca);
            bancoDeHorasWindow.ShowDialog();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarFuncionario(true);
        }

        private async void BtnDeletar_Click(object sender, RoutedEventArgs e)
        {
            HorasFuncionario horas = (HorasFuncionario)Listhoras.SelectedItem;

            HorasFuncionario horasFuncionario = await ClientApi.GetEmployeeHoursAsync(horas.Id).ConfigureAwait(true);

            if (MessageBox.Show("Deseja realmente deletar?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (horasFuncionario != null)
                {
                    if (horasFuncionario.Extras != (new TimeSpan(0, 0, 0)))
                    {
                        BancoDeHoras removerDoBancoDeHoras = new BancoDeHoras(-horasFuncionario.Extras, "Registro deletado", horasFuncionario.DataRegistro, Func.Registro);
                        await ClientApi.CreateBankHoursAsync(removerDoBancoDeHoras).ConfigureAwait(true);
                    }
                    else
                    {
                        BancoDeHoras JustificativaBancoDeHoras = new BancoDeHoras(new TimeSpan(0, 0, 0), "Registro deletado", horasFuncionario.DataRegistro, Func.Registro);
                        await ClientApi.CreateBankHoursAsync(JustificativaBancoDeHoras).ConfigureAwait(true);
                    }

                    await ClientApi.DeleteEmployeeHoursAsync(horas).ConfigureAwait(true);
                    await RefreshList(Func).ConfigureAwait(true);

                    horasFuncionario = await ClientApi.GetEmployeeHoursAsync(horas.Id).ConfigureAwait(true);
                    if (horasFuncionario == null)
                    {
                        MessageBox.Show("Registro deletado com sucesso!");
                    }
                }
                else
                {
                    MessageBox.Show("Não há registros na data selecionada!");
                }
            }
        }

        /// <summary>
        /// Evento click do botão detalhar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDetalhar_Click(object sender, RoutedEventArgs e)
        {
            GestaoDeFuncionarioWindow windows = new GestaoDeFuncionarioWindow(Func);
            windows.ShowDialog();
            BuscarFuncionario(false);
        }

        /// <summary>
        /// Recupera um funcionario do BD.
        /// </summary>
        private void BuscarFuncionario(bool novaBusca)
        {
            Funcionario funcionarioAtivo = Func;

            btnDeletar.IsEnabled = false;
            btnAlterar.IsEnabled = false;

            if (novaBusca)
            {
                Func = null;
            }

            if (Func == null)
            {
                ListarFuncionariosWindow listarFuncionariosWindow = new ListarFuncionariosWindow();
                listarFuncionariosWindow.ShowDialog();
                Func = listarFuncionariosWindow.FuncionarioAtual ?? funcionarioAtivo;
            }

            if (Func != null)
            {
                lblRegistro.Content = Func.Registro.ToString(CultureInfo.InvariantCulture);
                lblNome.Content = Func.Nome;

                if (Func.Ativo)
                {
                    btnAdicionar.IsEnabled = true;
                }
                else
                {
                    btnAdicionar.IsEnabled = false;
                }

                btnBancoHoras.IsEnabled = true;
                btnAtualizar.IsEnabled = true;
                RefreshList(Func).ConfigureAwait(true);
            }
            else
            {
                LimpaListBoxs();
                btnAtualizar.IsEnabled = false;
                btnAdicionar.IsEnabled = false;
                btnBancoHoras.IsEnabled = false;
            }
        }

        private void LimpaListBoxs()
        {
            lblRegistro.Content = "";
            lblNome.Content = "";
            lblBancoHoras.Content = "";
            lblHoraFolga.Content = "";
            lblTotalExtras.Content = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditarHora(HorasFuncionario horas)
        {
            EditarHorasWindow editarUltimo = new EditarHorasWindow(horas, Func);
            editarUltimo.ShowDialog();
            RefreshList(Func).ConfigureAwait(true);
        }

        private void Listhoras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Func != null && Func.Ativo)
            {
                btnAlterar.IsEnabled = true;
                btnDeletar.IsEnabled = true;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Atualiza os campos da janela.
        /// </summary>
        /// <param name="func">Objeto do tipo Funcionario recuperado do BD.</param>
        private async Task RefreshList(Funcionario func)
        {
            DateTime selecionada = DPdata.SelectedDate.Value;
            int fechamento = Empre.DiaFechamento;

            List<HorasFuncionario> horas;

            DateTime selecionadaBuscaInicio;
            DateTime selecionadaBuscaFim;

            if (selecionada.Day >= fechamento)
            {
                if (selecionada.Month == 12)
                {
                    selecionadaBuscaInicio = new DateTime(selecionada.Year, selecionada.Month, fechamento);
                    selecionadaBuscaFim = new DateTime(selecionada.Year + 1, 1, fechamento);
                }
                else
                {
                    selecionadaBuscaInicio = new DateTime(selecionada.Year, selecionada.Month, fechamento);
                    selecionadaBuscaFim = new DateTime(selecionada.Year, selecionada.Month + 1, fechamento);
                }
            }
            else
            {
                if (selecionada.Month == 1)
                {
                    selecionadaBuscaInicio = new DateTime(selecionada.Year - 1, 12, fechamento);
                    selecionadaBuscaFim = new DateTime(selecionada.Year, selecionada.Month, fechamento);
                }
                else
                {
                    selecionadaBuscaInicio = new DateTime(selecionada.Year, selecionada.Month - 1, fechamento);
                    selecionadaBuscaFim = new DateTime(selecionada.Year, selecionada.Month, fechamento);
                }
            }

            horas = await ClientApi.GetEmployeeHoursAsync(func, selecionadaBuscaFim, selecionadaBuscaInicio).ConfigureAwait(true);

            List<BancoDeHoras> bancoDeHoras = await ClientApi.GetBankHoursAsync(func).ConfigureAwait(true);

            Listhoras.ItemsSource = horas;

            TimeSpan totalHoraAlmoco = func.HoraRetornoAlmoco.Subtract(func.HoraSaidaAlmoco);
            TimeSpan horaTrabalho = func.HoraTermino.Subtract(func.HoraInicio).Subtract(totalHoraAlmoco);
            TimeSpan totalHorasFolga = new TimeSpan();
            TimeSpan totalHoras = new TimeSpan(horas.Sum(h => h.Extras.Ticks));
            TimeSpan totalBancoHoras = new TimeSpan(bancoDeHoras.Sum(b => b.HorasExtras.Ticks));

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

            if (totalHorasFolga.Days >= 0 && totalHorasFolga.Hours >= 0)
            {
                lblHoraFolga.Foreground = Brushes.LightGreen;
            }
            else
            {
                lblHoraFolga.Foreground = Brushes.IndianRed;
            }
        }

        /// <summary>
        /// Verifica se existe empresa cadastrada
        /// </summary>
        private async Task VerificaCadastroEmpresa()
        {
            try
            {
                Empresa empresa;

                try
                {
                    empresa = await ClientApi.GetCompanyAsync().ConfigureAwait(true);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                if (empresa != null)
                {
                    JanelaPrincipal.Title = $"Calculador de Horas - {empresa.RazaoSocial}";
                    Empre = empresa;
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
                            Empre = windows.Empresa;
                        }
                    } while (decisao);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Listhoras_LostFocus(object sender, RoutedEventArgs e)
        {
            btnDeletar.IsEnabled = false;
            btnAlterar.IsEnabled = false;
        }
    }
}
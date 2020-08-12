using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System;
using System.Net.Http;
using System.Windows;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Interaction logic for EditarHorasWindow.xaml
    /// </summary>
    public partial class EditarHorasWindow : Window
    {
        private readonly Funcionario Func;
        private readonly HorasFuncionario Horas;

        /// <summary>
        /// Janela de edição do último registro de hora adicionado
        /// </summary>
        public EditarHorasWindow(HorasFuncionario horasFuncionario, Funcionario funcionario)
        {
            Horas = horasFuncionario;
            Func = funcionario;
            InitializeComponent();
            RegistroParaAlterar();
        }

        /// <summary>
        /// Metodo para popular os itens do comboBox
        /// </summary>

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!VerificaPreenchimento())
            {
                MessageBox.Show("Existem campos não preechidos");
                return;
            }

            TimeSpan entrada = new TimeSpan(timePickerEntrada.SelectedTime.Value.Hour, timePickerEntrada.SelectedTime.Value.Minute, 0);
            TimeSpan saida = new TimeSpan(timePickerSaida.SelectedTime.Value.Hour, timePickerSaida.SelectedTime.Value.Minute, 0);
            TimeSpan saidaAlmoco = new TimeSpan(timePickerSaidaAlmoco.SelectedTime.Value.Hour, timePickerSaidaAlmoco.SelectedTime.Value.Minute, 0);
            TimeSpan retornoAlmoco = new TimeSpan(timePickerRetornoAlmoco.SelectedTime.Value.Hour, timePickerRetornoAlmoco.SelectedTime.Value.Minute, 0);

            TimeSpan totalHoraAlmoco = retornoAlmoco.Subtract(saidaAlmoco);
            TimeSpan horaTrabalho = Func.HoraTermino.Subtract(Func.HoraInicio).Subtract(totalHoraAlmoco);

            if (Horas.Extras != (new TimeSpan(0, 0, 0)))
            {
                BancoDeHoras removerDoBancoDeHoras = new BancoDeHoras(-Horas.Extras, "Horas extras removidas por edição de horas", Horas.DataRegistro, Func.Registro);
                await ClientApi.CreateBankHoursAsync(removerDoBancoDeHoras).ConfigureAwait(true);
            }

            Horas.Entrada = entrada;
            Horas.Saida = saida;
            Horas.HoraSaidaAlmoco = saidaAlmoco == new TimeSpan(0, 0, 0) ? Func.HoraSaidaAlmoco : saidaAlmoco;
            Horas.HoraRetornoAlmoco = retornoAlmoco == new TimeSpan(0, 0, 0) ? Func.HoraRetornoAlmoco : retornoAlmoco;
            Horas.CalculaExtras(horaTrabalho);

            if (Horas.Extras <= new TimeSpan(0, 10, 0) && Horas.Extras >= new TimeSpan(0, -10, 0))
            {
                Horas.Extras = new TimeSpan(0, 0, 0);
            }

            string justificativa = txtBoxJustificativa.Text;

            if (justificativa.Length < 20)
            {
                MessageBox.Show("A alteração só pode ser feita com uma justificativa de mínimo 20 caracteres.");
                return;
            }

            BancoDeHoras bancoDeHoras = new BancoDeHoras(Horas.Extras, justificativa, DateTime.Now, Func.Registro);

            try
            {
                await ClientApi.CreateBankHoursAsync(bancoDeHoras).ConfigureAwait(true);
                await ClientApi.UpdateEmployeeHoursAsync(Horas).ConfigureAwait(true);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }

            Close();
        }

        private void RegistroParaAlterar()
        {
            if (Horas != null)
            {
                timePickerEntrada.SelectedTime = new DateTime().Add(Horas.Entrada);
                timePickerSaidaAlmoco.SelectedTime = new DateTime().Add(Horas.HoraSaidaAlmoco);
                timePickerRetornoAlmoco.SelectedTime = new DateTime().Add(Horas.HoraRetornoAlmoco);
                timePickerSaida.SelectedTime = new DateTime().Add(Horas.Saida);
            }
        }

        private bool VerificaPreenchimento()
        {
            return timePickerEntrada.SelectedTime.HasValue && timePickerRetornoAlmoco.SelectedTime.HasValue && timePickerSaidaAlmoco.SelectedTime.HasValue && timePickerSaida.SelectedTime.HasValue;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
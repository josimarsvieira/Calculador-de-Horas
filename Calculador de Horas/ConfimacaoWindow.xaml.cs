using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Lógica interna para ConfimacaoWindow.xaml
    /// </summary>
    public partial class ConfirmacaoWindow : Window
    {
        private readonly Funcionario Func;
        private readonly HorasFuncionario Horas;

        /// <summary>
        /// Janela de confirmação de horas adicionadas
        /// </summary>
        /// <param name="horasFuncionario">Objeto hora funcionário para adição</param>
        /// <param name="funcionario">Objeto hora funcionário ao qual pertence as horas</param>
        public ConfirmacaoWindow(HorasFuncionario horasFuncionario, Funcionario funcionario)
        {
            InitializeComponent();
            Horas = horasFuncionario;
            if (Horas != null)
            {
                PreecncheTxtBox(Horas);
            }
            Func = funcionario;
        }

        private static async Task AddBancoDeHoras(BancoDeHoras banco)
        {
            try
            {
                await ClientApi.CreateBankHoursAsync(banco).ConfigureAwait(true);
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async Task AddMarcacaoPonto(HorasFuncionario horas)
        {
            if (horas.Extras.Minutes != 0 || horas.Extras.Hours != 0)
            {
                BancoDeHoras bancoDeHoras = new BancoDeHoras(horas.Extras, "Horas Extras", horas.DataRegistro, Func.Registro);
                await AddBancoDeHoras(bancoDeHoras).ConfigureAwait(true);
            }
            try
            {
                await ClientApi.CreateEmployeeHoursAsync(horas).ConfigureAwait(true);
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Button_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Button_Confirmar_Click(object sender, RoutedEventArgs e)
        {
            await AddMarcacaoPonto(Horas).ConfigureAwait(true);

            Close();
        }

        private void Button_Reducao_Click(object sender, RoutedEventArgs e)
        {
            Horas.CalculaExtras(Func.HoraTermino.Subtract(Func.HoraInicio).Subtract(Func.HoraRetornoAlmoco.Subtract(Func.HoraSaidaAlmoco)) / 2);
            PreecncheTxtBox(Horas);

            txtReducao.Text = Properties.Resources.ConfirmarReducao;
        }

        private void PreecncheTxtBox(HorasFuncionario horasFuncionario)
        {
            txtEntrada.Text = horasFuncionario.Entrada.ToString();
            txtSaidaAlmoco.Text = horasFuncionario.HoraSaidaAlmoco.ToString();
            txtRetornoAlmoco.Text = horasFuncionario.HoraRetornoAlmoco.ToString();
            txtSaida.Text = horasFuncionario.Saida.ToString();
            txtExtras.Text = horasFuncionario.Extras.ToString();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
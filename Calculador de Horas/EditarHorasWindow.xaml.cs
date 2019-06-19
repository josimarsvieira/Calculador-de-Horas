using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Calculador_de_Horas
{
    /// <summary>
    /// Interaction logic for EditarHorasWindow.xaml
    /// </summary>
    public partial class EditarHorasWindow : Window
    {
        public static DateTime dataAlterar;
        /// <summary>
        /// Janela de edição do último registro de hora adicionado
        /// </summary>
        public EditarHorasWindow(DateTime data)
        {
            dataAlterar = data;
            InitializeComponent();
            PreencheComboBox();
            RegistroParaAlterar();
        }

        /// <summary>
        /// Metodo para popular os itens do comboBox
        /// </summary>
        private void PreencheComboBox()
        {
            List<string> Horas = new List<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            List<string> Minutos = new List<string> { "00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29",
                "30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47","48","49","50","51","52","53","54","55","56","57","58","59"};

            cbHoraEntrada.ItemsSource = Horas;
            cbMinutosEntrada.ItemsSource = Minutos;
            cbHoraSaida.ItemsSource = Horas;
            cbMinutosSaida.ItemsSource = Minutos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario funcionario = dbContext.BuscarFuncionario(TranferenciaDados.Registro);
                HorasFuncionario horasFuncionario = dbContext.BuscarRegistro(funcionario, dataAlterar);
                TimeSpan entrada = new TimeSpan(int.Parse(cbHoraEntrada.SelectionBoxItem.ToString()), int.Parse(cbMinutosEntrada.SelectionBoxItem.ToString()), 0);
                TimeSpan saida = new TimeSpan(int.Parse(cbHoraSaida.SelectionBoxItem.ToString()), int.Parse(cbMinutosSaida.SelectionBoxItem.ToString()), 0);

                TimeSpan totalHoraAlmoco = funcionario.HoraAlmocoRetorno.Subtract(funcionario.HoraAlmocoSaida);
                TimeSpan horaTrabalho = funcionario.HoraTermino.Subtract(funcionario.HoraIncio).Subtract(totalHoraAlmoco);

                if (horasFuncionario.Extras != (new TimeSpan(0, 0, 0)))
                {
                    BancoDeHoras removerDoBancoDeHoras = new BancoDeHoras(-horasFuncionario.Extras, "Horas extras removidas", horasFuncionario.DataRegistro);
                    funcionario.AtualizarBancoHoras(removerDoBancoDeHoras);
                    dbContext.UpdateBanco(funcionario);
                }

                horasFuncionario.Entrada = entrada;
                horasFuncionario.Saida = saida;
                horasFuncionario.CalculaExtras(horaTrabalho.Add(totalHoraAlmoco));

                if (horasFuncionario.Extras <= new TimeSpan(0, 10, 0) && horasFuncionario.Extras >= new TimeSpan(0, -10, 0))
                {
                    horasFuncionario.Extras = new TimeSpan(0, 0, 0);
                }

                string justificativa = txtBoxJustificativa.Text;

                if (justificativa.Length < 20)
                {
                    MessageBox.Show("A alteração só pode ser feita com uma justificativa de mínimo 20 caracteres.");
                    return;
                }

                BancoDeHoras bancoDeHoras = new BancoDeHoras(horasFuncionario.Extras, justificativa, DateTime.Now);
                funcionario.AtualizarBancoHoras(bancoDeHoras);

                dbContext.UpdateBanco(funcionario);
                dbContext.UpdateHora(horasFuncionario);
            }

            MessageBox.Show("Alteração efetuada");
            Close();
        }

        private HorasFuncionario RegistroParaAlterar()
        {
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Funcionario funcionario = dbContext.BuscarFuncionario(TranferenciaDados.Registro);
                HorasFuncionario horasFuncionario = dbContext.BuscarRegistro(funcionario, dataAlterar);

                if (horasFuncionario != null)
                {
                    cbHoraEntrada.SelectedIndex = horasFuncionario.Entrada.Hours;
                    cbMinutosEntrada.SelectedIndex = horasFuncionario.Entrada.Minutes;
                    cbHoraSaida.SelectedIndex = horasFuncionario.Saida.Hours;
                    cbMinutosSaida.SelectedIndex = horasFuncionario.Saida.Minutes;
                }

                return horasFuncionario;
            }
        }
    }
}

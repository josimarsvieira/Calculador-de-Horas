using CalculadorDeHoras.Database;
using CalculadorDeHoras.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

namespace CalculadorDeHoras
{
    /// <summary>
    /// Lógica interna para NovaEmpresaWindow.xaml..
    /// </summary>
    public partial class NovaEmpresaWindow : Window
    {
        /// <summary>
        /// Construtor da janela para inclusão de empresa
        /// </summary>
        public NovaEmpresaWindow()
        {
            InitializeComponent();
            PreencherComboDia();
        }

        /// <summary>
        /// Objeto empresa ativo na aplicação.
        /// </summary>
        public Empresa Empresa { get; set; }

        /// <summary>
        /// Evento click do botão salvar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Empresa = new Empresa(txtRazao.Text, txtCNPJ.Text, cbDia.SelectedIndex);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Favor conferir os dados informados" + ex.Message);
                return;
            }

            Empresa busca = await ClientApi.GetCompanyAsync().ConfigureAwait(true);

            if (busca != null)
            {
                switch (MessageBox.Show("Cadastro já existente!\nDeseja Atualizar com esses dados?", "Confirmar alteração", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            await ClientApi.UpdateCompanyAsync(Empresa).ConfigureAwait(true);
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
                    await ClientApi.CreateCompanyAsync(Empresa).ConfigureAwait(true);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("Erro ao gravar os dados, contate o suporte\nPara uso do TI. Menssagem: " + ex.Message);
                    Close();
                }

                MessageBox.Show("Cadastro efetuado com sucesso.");
                Close();
            }
        }

        /// <summary>
        /// Preenche os campos da interface
        /// </summary>
        /// <param name="busca"></param>
        private void PreencherCampos(Empresa busca)
        {
            txtRazao.Text = busca.RazaoSocial;
            txtCNPJ.Text = busca.CNPJ;
            cbDia.SelectedIndex = busca.DiaFechamento - 1;
        }

        /// <summary>
        /// Popula o comboBox da interface
        /// </summary>
        private void PreencherComboDia()
        {
            List<string> dias = new List<string> { "00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29",
                "30" };
            cbDia.ItemsSource = dias;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
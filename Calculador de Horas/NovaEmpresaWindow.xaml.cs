using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Calculador_de_Horas
{

    /// <summary>
    /// Lógica interna para NovaEmpresaWindow.xaml
    /// </summary>
    public partial class NovaEmpresaWindow : Window
    {
        public Empresa Empresa { get; private set; }

        public NovaEmpresaWindow()
        {
            InitializeComponent();
            PreencherComboDia();
        }

        /// <summary>
        /// Popula os comboBox da interface
        /// </summary>
        private void PreencherComboDia()
        {
            List<string> dias = new List<string> { "00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29",
                "30" };
            cbDia.ItemsSource = dias;
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
        /// Evento click do botão salvar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Empresa = new Empresa(txtRazao.Text, txtCNPJ.Text, int.Parse(cbDia.SelectedIndex.ToString()));
            }
            catch (FormatException)
            {
                MessageBox.Show("Favor conferir os dados informados");
                return;
            }

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Empresa busca = dbContext.BuscarEmpresa();

                if (busca != null)
                {
                    switch (MessageBox.Show("Cadastro já existente!\nDeseja Atualizar com esses dados?", "Confirmar alteração", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                    {
                        case MessageBoxResult.Yes:
                            try
                            {
                                dbContext.AtualizarEmpresa(Empresa, busca);
                                MessageBox.Show("Cadastro efetuado com sucesso.");
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
                        dbContext.SalvarEmpresa(Empresa);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao gravar os dados, contate o suporte\nPara uso do TI. Menssagem: " + ex.Message);
                    }
                    MessageBox.Show("Cadastro efetuado com sucesso.");
                    Close();
                }
            }
        }


    }
}

﻿using Calculador_de_Horas.Database;
using Calculador_de_Horas.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
namespace Calculador_de_Horas
{
    /// <summary>
    /// Lógica interna para ListarFuncionariosWindow.xaml
    /// </summary>
    public partial class ListarFuncionariosWindow : Window
    {
        public ListarFuncionariosWindow()
        {
            InitializeComponent();
            PreencheListBox();
        }

        /// <summary>
        /// Preenche a lista de funcionarios
        /// </summary>
        public void PreencheListBox()
        {
            lbFuncionarios.Items.Clear();

            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                List<Funcionario> funcionario;

                try
                {
                    funcionario = new List<Funcionario>();
                    funcionario = dbContext.BuscarFuncionario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (funcionario != null)
                {
                    foreach (Funcionario func in funcionario)
                    {
                        lbFuncionarios.Items.Add($"{func.Registro} - {func.Nome}");
                    }
                }
                else
                {
                    lbFuncionarios.Items.Add("Sem registros");
                }
            }
        }

        /// <summary>
        /// Evento click do botão selecionar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            string selecionado = lbFuncionarios.SelectedItem.ToString().Remove(4);
            int registro = int.Parse(selecionado);
            TranferenciaDados.Registro = registro;
            Close();
        }

        /// <summary>
        /// Evento click do botão novo funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNovo_Click(object sender, RoutedEventArgs e)
        {
            NovoFuncionarioWindow windows = new NovoFuncionarioWindow("");
            windows.ShowDialog();
            PreencheListBox();
        }

        /// <summary>
        /// Evento click do botão editar funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            string selecionado = lbFuncionarios.SelectedItem.ToString().Remove(4);
            NovoFuncionarioWindow windows = new NovoFuncionarioWindow(selecionado);
            windows.ShowDialog();
            PreencheListBox();

        }
    }
}
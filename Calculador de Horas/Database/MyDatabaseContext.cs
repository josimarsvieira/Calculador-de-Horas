using Calculador_de_Horas.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculador_de_Horas.Database
{
    /// <summary>
    /// Classe de acesso e alterações no banco de dados
    /// </summary>
    public class MyDatabaseContext : DbContext
    {
        /// <summary>
        /// Representação da tabela Horas dos Funcionarios no banco de dados.
        /// </summary>
        public DbSet<HorasFuncionario> HorasFuncionarios { get; private set; }
        /// <summary>
        /// Representação da tabela funcionario no banco de dados.
        /// </summary>
        public DbSet<Funcionario> Funcionario { get; private set; }
        /// <summary>
        /// Representação da tabela Banco de Horas no banco de dados.
        /// </summary>
        public DbSet<BancoDeHoras> BancoDeHoras { get; private set; }

        public DbSet<Empresa> Empresa { get; private set; }

        /// <summary>
        /// Metodo que fornece a String de conexão com o BD.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyDatabase.sqlite");
        }

        /// <summary>
        /// Metodo responsavel pelo salvamento de Funcionario no BD.
        /// </summary>
        /// <param name="funcionario"></param>
        public void SalvarFuncionario(Funcionario funcionario)
        {
            Funcionario.Add(funcionario);
            SaveChanges();
        }

        /// <summary>
        /// Metodo responsavel pela atualização de Funcionario no BD.
        /// </summary>
        /// <param name="funcionario">Objeto do tipo Funcionario com os novos dados.</param>
        /// <param name="busca">Objeto do tipo Funcionario recuperado do BD e que vai ser atualizado.</param>
        public void AtualizarFuncionario(Funcionario funcionario, Funcionario busca)
        {
            Entry(busca).CurrentValues.SetValues(busca.Nome = funcionario.Nome);
            Entry(busca).CurrentValues.SetValues(busca.Funcao = funcionario.Funcao);
            Entry(busca).CurrentValues.SetValues(busca.HoraIncio = funcionario.HoraIncio);
            Entry(busca).CurrentValues.SetValues(busca.HoraTermino = funcionario.HoraTermino);
            Entry(busca).CurrentValues.SetValues(busca.HoraAlmocoSaida = funcionario.HoraAlmocoSaida);
            Entry(busca).CurrentValues.SetValues(busca.HoraAlmocoRetorno = funcionario.HoraAlmocoRetorno);
            SaveChanges();
        }

        /// <summary>
        /// Localiza um funcionario especifico no banco de dados.
        /// </summary>
        /// <param name="numeroRegistro">Numero do registro do funcionario a ser localizado.</param>
        /// <returns>Retorna um objeto do tipo Funcionario.</returns>
        
        public Funcionario BuscarFuncionario(int numeroRegistro)
        {
            Funcionario result = Funcionario.AsQueryable().Where(x => x.Registro == numeroRegistro).SingleOrDefault();
            return result;

        }

        public List<Funcionario> BuscarFuncionario()
        {
            IEnumerable todosFuncionarios = Funcionario.AsQueryable().Where(x => x.Registro != 0);

            List<Funcionario> funcionarios = new List<Funcionario>();

            foreach (Funcionario func in todosFuncionarios)
            {
                funcionarios.Add(func);
            }

            return funcionarios;
        }

        /// <summary>
        /// Metodo responsavel pela adição de horas ao Funcionario.
        /// </summary>
        /// <param name="funcionario">Objeto do tipo funcionario recuperado do BD para a adição.</param>
        public void UpdateFuncionario(Funcionario funcionario)
        {
            Funcionario.Update(funcionario);
            SaveChanges();
        }

        /// <summary>
        /// Recupera todos os registros de horas de um funcionario especifico.
        /// </summary>
        /// <param name="funcionario">Objeto do tipo funcionario recuperado do BD.</param>
        /// <param name="mes">Mes referente ao periodo desejado para recuperação.</param>
        /// <param name="ano">Ano referente ao periodo desejado para recuperação.</param>
        /// <returns>Retorna um IEnumerable contendo uma lista de todos os registros encontrados.</returns>
        public IEnumerable BuscaCartaoPonto(Funcionario funcionario, DateTime dataBusca)
        {
            IEnumerable Cartao = HorasFuncionarios.AsQueryable().Where(x => x.FuncionarioId == funcionario.Id &&
            x.DataRegistro.Month == dataBusca.Month &&
            x.DataRegistro.Year == dataBusca.Year).OrderBy(x => x.DataRegistro);
            return Cartao;
        }

        public IEnumerable BuscaCartaoPonto(Funcionario funcionario, DateTime dataFinal, DateTime dataInicial)
        {
            IEnumerable Cartao = HorasFuncionarios.AsQueryable().Where(x => x.FuncionarioId == funcionario.Id &&
            x.DataRegistro <= dataFinal &&
            x.DataRegistro >= dataInicial).OrderBy(x => x.DataRegistro);
            return Cartao;
        }

        /// <summary>
        /// Recupera o banco de horas de um funcionario especifico.
        /// </summary>
        /// <param name="funcionario">Objeto do tipo funcionario recuperado do BD.</param>
        /// <returns>Retorna um IEnumerable contendo uma lista de todos os registros encontrados.</returns>
        public IEnumerable BuscaBancoDeHoras(Funcionario funcionario)
        {
            IEnumerable Banco = BancoDeHoras.AsQueryable().Where(x => x.FuncionarioId == funcionario.Id).OrderBy(x => x.DataRegistro);
            return Banco;
        }

        /// <summary>
        /// Recupera o banco de horas de um funcionario especifico, filtrado pelo mês e ano.
        /// </summary>
        /// <param name="funcionario">Objeto do tipo funcionario recuperado do BD.</param>
        /// <param name="mes">Mes referente ao periodo desejado para recuperação.</param>
        /// <param name="ano">Ano referente ao periodo desejado para recuperação.</param>
        /// <returns>Retorna um IEnumerable contendo uma lista de todos os registros encontrados.</returns>
        public IEnumerable BuscaBancoDeHorasFiltrado(Funcionario funcionario, DateTime dataBusca)
        {
            IEnumerable BancoFiltrado = BancoDeHoras.AsQueryable().Where(x => x.FuncionarioId == funcionario.Id &&
            x.DataRegistro.Month == dataBusca.Month &&
            x.DataRegistro.Year == dataBusca.Year);
            return BancoFiltrado;
        }

        /// <summary>
        /// Atualiza um registro de horas especifico de um funcionario.
        /// </summary>
        /// <param name="horasFuncionario">Objeto do tipo HorasFuncionario recuperado do BD.</param>
        public void UpdateHora(HorasFuncionario horasFuncionario)
        {
            HorasFuncionarios.Update(horasFuncionario);
            SaveChanges();
        }

        /// <summary>
        /// Adiciona um novo registro no banco de horas de um funcionario. 
        /// </summary>
        /// <param name="funcionario">Objeto do tipo Funcionario recuperado do BD.</param>
        public void UpdateBanco(Funcionario funcionario)
        {
            Entry(funcionario).CurrentValues.SetValues(funcionario.BancoDeHoras);
            SaveChanges();
        }

        public Empresa BuscarEmpresa()
        {
            Empresa busca = Empresa.AsQueryable().Where(x => x.RazaoSocial != null).SingleOrDefault();
            return busca;
        }

        public void SalvarEmpresa(Empresa empresa)
        {
            Empresa.Add(empresa);
            SaveChanges();
        }

        public void AtualizarEmpresa(Empresa empresa, Empresa busca)
        {
            Entry(busca).CurrentValues.SetValues(busca.RazaoSocial = empresa.RazaoSocial);
            Entry(busca).CurrentValues.SetValues(busca.CNPJ = empresa.CNPJ);
            Entry(busca).CurrentValues.SetValues(busca.DiaFechamento = empresa.DiaFechamento);
            SaveChanges();
        }

        public HorasFuncionario BuscarRegistro(Funcionario funcionario, DateTime dataAlterar)
        {
            HorasFuncionario hora = HorasFuncionarios.AsQueryable().Where(x => x.FuncionarioId == funcionario.Id && x.DataRegistro == dataAlterar).SingleOrDefault();
            return hora;
        }

        public void RemoveHora(Funcionario funcionario, DateTime dataRemover)
        {
            Remove(funcionario.CartaoPonto.Single(x => x.DataRegistro == dataRemover));
            SaveChanges();
        }
    }
}

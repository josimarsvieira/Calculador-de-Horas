using CalculadorDeHoras.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CalculadorDeHoras.Database
{
    /// <summary>
    /// Classe de acesso e alterações no banco de dados
    /// </summary>
    public static class ClientApi
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static ClientApi()
        {
            RunApi().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adicina horas no banco de horas
        /// </summary>
        /// <param name="bankHoursIn">Objeto banco de horas a ser adicionado</param>
        /// <returns>Retorna uma uri contendo o endereço da api do novo registro</returns>
        public static async Task<Uri> CreateBankHoursAsync(BancoDeHoras bankHoursIn)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/bankHours/", bankHoursIn).ConfigureAwait(true);
            try
            {
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Adiciona uma nova empresa
        /// </summary>
        /// <param name="companyIn">Objeto empresa a ser adicionado</param>
        /// <returns>Retorna uma uri contendo o endereço da api do novo registro</returns>
        public static async Task<Uri> CreateCompanyAsync(Empresa companyIn)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/company/", companyIn).ConfigureAwait(true);
            try
            {
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Adiciona um novo funcionário
        /// </summary>
        /// <param name="employeeIn">Objeto funcionário a ser adicionado</param>
        /// <returns>Retorna uma uri contendo o endereço da api do novo registro</returns>
        public static async Task<Uri> CreateEmployeeAsync(Funcionario employeeIn)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/employee", employeeIn).ConfigureAwait(true);

            try
            {
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Adiciona um registro de horas a um funcionário
        /// </summary>
        /// <param name="employeeHoursIn">Objeto horas funcionário a ser adicionado</param>
        /// <returns>Retorna uma tarefa</returns>
        public static async Task CreateEmployeeHoursAsync(HorasFuncionario employeeHoursIn)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/employeeHours/", employeeHoursIn).ConfigureAwait(true);

            try
            {
                response.EnsureSuccessStatusCode();

                Console.WriteLine(response.Headers.Location);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Deleta um registro de horas de um funcionário
        /// </summary>
        /// <param name="employeeHoursToDelete">Objeto horas funcionario a ser deletado</param>
        /// <returns>Retorna o codigo http com o status da transação</returns>
        public static async Task<HttpStatusCode> DeleteEmployeeHoursAsync(HorasFuncionario employeeHoursToDelete)
        {
            if (employeeHoursToDelete == null)
            {
                return HttpStatusCode.NotFound;
            }

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employeeHours/{employeeHoursToDelete.Id}");

            HttpResponseMessage response = await httpClient.DeleteAsync(uri).ConfigureAwait(true);
            return response.StatusCode;
        }

        /// <summary>
        /// Obtem uma lista contendo todos os registros do banco de horas de um funcionário
        /// </summary>
        /// <param name="employee">Objeto funcionario ao qual pertence o banco de horas</param>
        /// <returns>Retorna uma lista contendo de objetos banco de horas</returns>
        public static async Task<List<BancoDeHoras>> GetBankHoursAsync(Funcionario employee)
        {
            List<BancoDeHoras> bankHours = null;

            if (employee == null)
            {
                return bankHours;
            }

            Uri uri = new Uri(httpClient.BaseAddress + $"api/bankHours/{employee.Registro}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var bank = await response.Content.ReadAsAsync<IEnumerable<BancoDeHoras>>().ConfigureAwait(true);
                bankHours = bank.ToList();
            }

            return bankHours;
        }

        /// <summary>
        /// Obtem uma lista contendo os registros de um mês do banco de horas de um funcionário
        /// </summary>
        /// <param name="employee">Objeto funcionario ao qual pertence o banco de horas</param>
        /// <param name="dateSearch">Mês para busca</param>
        /// <returns>Retorna uma lista de objetos banco de horas</returns>
        public static async Task<List<BancoDeHoras>> GetBankHoursFilteredAsync(Funcionario employee, DateTime dateSearch)
        {
            List<BancoDeHoras> bankHours = null;

            if (employee == null)
            {
                return bankHours;
            }

            Uri uri = new Uri(httpClient.BaseAddress + $"api/bankHours/{employee.Registro}&{dateSearch.Year}-{dateSearch.Month}-{dateSearch.Day}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var bank = await response.Content.ReadAsAsync<IEnumerable<BancoDeHoras>>().ConfigureAwait(true);
                bankHours = bank.ToList();
            }

            return bankHours;
        }

        /// <summary>
        /// Obtem uma empresa
        /// </summary>
        /// <returns>Retorna um objeto empresa</returns>
        public static async Task<Empresa> GetCompanyAsync()
        {
            Empresa company;

            Uri uri = new Uri(httpClient.BaseAddress + "api/company");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (true)
            {
                company = await response.Content.ReadAsAsync<Empresa>().ConfigureAwait(true);
            }

            return company;
        }

        /// <summary>
        /// Obtem um funcionário por id
        /// </summary>
        /// <param name="id">Id do funcionario</param>
        /// <returns>Retorna um objeto funcionário</returns>
        public static async Task<Funcionario> GetEmployeeAsync(string id)
        {
            Funcionario employee = null;

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employee/{id}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                employee = await response.Content.ReadAsAsync<Funcionario>().ConfigureAwait(true);
            }

            return employee;
        }

        /// <summary>
        /// Obtem um funcionário por registro
        /// </summary>
        /// <param name="empId">Numero do registro do funcionário</param>
        /// <returns>Retorna um objeto funcionário</returns>
        public static async Task<Funcionario> GetEmployeeAsync(int empId)
        {
            Funcionario employee = null;

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employee/00{empId}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                employee = await response.Content.ReadAsAsync<Funcionario>().ConfigureAwait(true);
            }

            return employee;
        }

        /// <summary>
        /// Obtem os registro de horas em um periodo de um funcionário.
        /// </summary>
        /// <param name="employee">Objeto funcionário ao qual pertence os registros</param>
        /// <param name="dateEnd">Data final do periodo</param>
        /// <param name="dateStart">Data inicial do periodo</param>
        /// <returns>Retorna uma lista contendo objetos horas funcionário</returns>
        public static async Task<List<HorasFuncionario>> GetEmployeeHoursAsync(Funcionario employee, DateTime dateEnd, DateTime dateStart)
        {
            List<HorasFuncionario> employeeHours = null;

            if (employee == null)
            {
                return employeeHours;
            }

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employeeHours/{employee.Registro}&{dateEnd.Year}-{dateEnd.Month}-{dateEnd.Day}&{dateStart.Year}-{dateStart.Month}-{dateStart.Day}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var empHours = await response.Content.ReadAsAsync<IEnumerable<HorasFuncionario>>().ConfigureAwait(true);
                employeeHours = empHours.ToList();
            }

            return employeeHours;
        }

        /// <summary>
        /// Obtem as horas de um mês de um fucionário.
        /// </summary>
        /// <param name="employee">Objeto funcionário ao qual pertence os registros</param>
        /// <param name="monthSearch">Mês para busca</param>
        /// <returns>Retorna uma lista contendo objetos horas funcionário</returns>
        public static async Task<List<HorasFuncionario>> GetEmployeeHoursAsync(Funcionario employee, DateTime monthSearch)
        {
            List<HorasFuncionario> employeeHours = null;

            if (employee == null)
            {
                return employeeHours;
            }

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employeeHours/{employee.Registro}&{monthSearch.Year}-{monthSearch.Month}-{monthSearch.Day}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var empHours = await response.Content.ReadAsAsync<IEnumerable<HorasFuncionario>>().ConfigureAwait(true);
                employeeHours = empHours.ToList();
            }

            return employeeHours;
        }

        /// <summary>
        /// Otem um resgitro de horas.
        /// </summary>
        /// <param name="id">Id do registro de horas</param>
        /// <returns>Retorna um objeto horas funcionário</returns>
        public static async Task<HorasFuncionario> GetEmployeeHoursAsync(string id)
        {
            HorasFuncionario employeeHours = null;

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employeeHours/{id}");

            HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                employeeHours = await response.Content.ReadAsAsync<HorasFuncionario>().ConfigureAwait(true);
            }

            return employeeHours;
        }

        /// <summary>
        /// Obtem todos os funcionários cadastrados.
        /// </summary>
        /// <returns>Retorna uma lista contendo objetos funcionário</returns>
        public static async Task<List<Funcionario>> GetEmployeesAsync()
        {
            List<Funcionario> employees = null;

            HttpResponseMessage response;

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employee/");

            response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var emp = await response.Content.ReadAsAsync<IEnumerable<Funcionario>>().ConfigureAwait(true);
                employees = emp.ToList();
            }

            return employees;
        }

        /// <summary>
        /// Obtem todos os funcionários ativos cadastrados.
        /// </summary>
        /// <param name="ativo">True para ativos e False para inativos</param>
        /// <returns>Retorna uma lista contendo objetos funcionário</returns>
        public static async Task<List<Funcionario>> GetEmployeesAsync(bool ativo)
        {
            List<Funcionario> employees = null;

            HttpResponseMessage response;

            Uri uri = new Uri(httpClient.BaseAddress + $"api/employee/{ativo}");

            response = await httpClient.GetAsync(uri).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var emp = await response.Content.ReadAsAsync<IEnumerable<Funcionario>>().ConfigureAwait(true);
                employees = emp.ToList();
            }

            return employees;
        }

        /// <summary>
        /// Atualiza o registro de uma empresa.
        /// </summary>
        /// <param name="companyIn">Objeto empresa alterado</param>
        /// <returns>Retorna um objeto empresa com as alterações feitas</returns>
        public static async Task<Empresa> UpdateCompanyAsync(Empresa companyIn)
        {
            if (companyIn == null)
            {
                return null;
            }
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/company/{companyIn.Id}", companyIn).ConfigureAwait(true);
            response.EnsureSuccessStatusCode();

            companyIn = await response.Content.ReadAsAsync<Empresa>().ConfigureAwait(true);

            return companyIn;
        }

        /// <summary>
        /// Atualiza o registro de um funcionário.
        /// </summary>
        /// <param name="employeeIn">Objeto funcionário alterado</param>
        /// <returns>Retorna um objeto funcionário com as alterações feitas</returns>
        public static async Task<Funcionario> UpdateEmployeeAsync(Funcionario employeeIn)
        {
            if (employeeIn == null)
            {
                return null;
            }

            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/employee/{employeeIn.Id}", employeeIn).ConfigureAwait(true);

            response.EnsureSuccessStatusCode();

            employeeIn = await response.Content.ReadAsAsync<Funcionario>().ConfigureAwait(true);

            return employeeIn;
        }

        /// <summary>
        /// Atualiza um registro de horas de um funcionário.
        /// </summary>
        /// <param name="horasFuncionarioIn">Objeto horas funcionário alterado</param>
        /// <returns>Retorna um objeto funconário com as alterações feitas</returns>
        public static async Task<HorasFuncionario> UpdateEmployeeHoursAsync(HorasFuncionario horasFuncionarioIn)
        {
            if (horasFuncionarioIn == null)
            {
                return null;
            }

            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/employeeHours/{horasFuncionarioIn.Id}", horasFuncionarioIn).ConfigureAwait(true);
            response.EnsureSuccessStatusCode();

            horasFuncionarioIn = await response.Content.ReadAsAsync<HorasFuncionario>().ConfigureAwait(true);

            return horasFuncionarioIn;
        }

        /// <summary>
        /// Conecta a aplicação a API
        /// </summary>
        /// <returns>Retorna uma tarefa Task</returns>
        private static async Task RunApi()
        {
            httpClient.BaseAddress = new Uri("https://localhost:44337/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
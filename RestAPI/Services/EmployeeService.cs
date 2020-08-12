using MongoDB.Driver;
using RestAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestAPI.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Funcionario> _employee;

        public EmployeeService(ICalcHours settings)
        {
            if (settings != null)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _employee = database.GetCollection<Funcionario>(settings.EmployeeCollection);
            }
        }

        public void Create(Funcionario employeeIn)
        {
            if (employeeIn != null)
            {
                Funcionario employee = Get(employeeIn.Registro);

                if (employee == null)
                {
                    _employee.InsertOne(employeeIn);
                }
            }
        }

        public List<Funcionario> Get() => _employee.Find(employee => true).SortBy(employee => employee.Registro).ToList();

        public List<Funcionario> Get(bool ativo) => _employee.Find(employee => employee.Ativo == ativo).SortBy(employee => employee.Registro).ToList();

        public Funcionario Get(string id) => _employee.Find(employee => employee.Id == id).FirstOrDefault();

        public Funcionario Get(int empId) => _employee.Find(employee => employee.Registro == empId).FirstOrDefault();

        public void Update(string id, Funcionario employeeIn) => _employee.ReplaceOne(employee => employee.Id == id, employeeIn);
    }
}
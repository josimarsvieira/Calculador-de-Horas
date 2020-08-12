using MongoDB.Driver;
using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestAPI.Services
{
    public class EmployeeHoursService
    {
        private readonly IMongoCollection<HorasFuncionario> _employeeHoras;

        public EmployeeHoursService(ICalcHours settings)
        {
            if (settings != null)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _employeeHoras = database.GetCollection<HorasFuncionario>(settings.EmployeeHoursCollection);
            }
        }

        public List<HorasFuncionario> Get(int id) => _employeeHoras.Find(empHours => empHours.Funcionario == id).SortBy(empHours => empHours.DataRegistro).ToList();

        public List<HorasFuncionario> Get(int id, DateTime monthSearch) =>
            _employeeHoras.Find(empHours => empHours.Funcionario == id && empHours.DataRegistro <=
            new DateTime(monthSearch.Year, monthSearch.Month, 1).AddMonths(1)
            && empHours.DataRegistro >= new DateTime(monthSearch.Year, monthSearch.Month, 1)).SortBy(empHours => empHours.DataRegistro).ToList();

        public List<HorasFuncionario> Get(int id, DateTime dateEnd, DateTime dateStart)
        {
            return _employeeHoras.Find(empHours => empHours.Funcionario == id &&
                empHours.DataRegistro < dateEnd && empHours.DataRegistro >= dateStart)
                    .SortBy(emphours => emphours.DataRegistro).ToList();
        }

        public HorasFuncionario Get(string id) => _employeeHoras.Find(hour => hour.Id == id).FirstOrDefault();

        public HorasFuncionario GetFilter(int id, DateTime dateAlter) => _employeeHoras.Find(empHours => empHours.Funcionario == id && empHours.DataRegistro == dateAlter).FirstOrDefault();

        public void Create(HorasFuncionario employeerHours) => _employeeHoras.InsertOne(employeerHours);

        public void Update(HorasFuncionario employeerHours) => _employeeHoras.ReplaceOne(empHours => empHours.Funcionario == employeerHours.Funcionario && empHours.DataRegistro == employeerHours.DataRegistro, employeerHours);

        public void Remove(string id) => _employeeHoras.DeleteOne(empHours => empHours.Id == id);
    }
}
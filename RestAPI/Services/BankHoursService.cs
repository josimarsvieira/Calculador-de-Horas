using MongoDB.Driver;
using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestAPI.Services
{
    public class BankHoursService
    {
        private readonly IMongoCollection<BancoDeHoras> _bankHoursService;

        public BankHoursService(ICalcHours settings)
        {
            if (settings != null)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _bankHoursService = database.GetCollection<BancoDeHoras>(settings.BankHoursCollection);
            }
        }

        public BancoDeHoras Create(BancoDeHoras bankIn)
        {
            _bankHoursService.InsertOne(bankIn);

            return bankIn;
        }

        public List<BancoDeHoras> Get(int empId) => _bankHoursService.Find(bank => bank.Funcionario == empId).SortBy(bank => bank.DataRegistro).ToList();

        public BancoDeHoras Get(string id) => _bankHoursService.Find<BancoDeHoras>(bank => bank.Id == id).FirstOrDefault();

        public List<BancoDeHoras> Get(int empId, DateTime dateSearch) => _bankHoursService.Find(bank => bank.Funcionario == empId && bank.DataRegistro <= dateSearch && bank.DataRegistro >= dateSearch.AddMonths(-1)).SortBy(bank => bank.DataRegistro).ToList();

        public void Remove(BancoDeHoras bancoDeHorasIn) => _bankHoursService.DeleteOne(bancoDeHoras => bancoDeHoras.Id == bancoDeHorasIn.Id);

        public void Update(string id, BancoDeHoras bancoDeHourasIn) => _bankHoursService.ReplaceOne(bancoDeHouras => bancoDeHourasIn.Id == id, bancoDeHourasIn);
    }
}
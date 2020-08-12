using MongoDB.Driver;
using RestAPI.Models;
using System.Linq;

namespace RestAPI.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Empresa> _company;

        public CompanyService(ICalcHours settings)
        {
            if (settings != null)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _company = database.GetCollection<Empresa>(settings.CompanyCollection);
            }
        }

        public void Create(Empresa company) => _company.InsertOne(company);

        public Empresa Get() => _company.Find(company => true).FirstOrDefault();

        public Empresa Get(string id) => _company.Find(company => company.Id == id).FirstOrDefault();

        public void Remove(string id) => _company.DeleteOne(company => company.Id == id);

        public void Update(string id, Empresa companyIn) => _company.ReplaceOne(company => company.Id == id, companyIn);
    }
}
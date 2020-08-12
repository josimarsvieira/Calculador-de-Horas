using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestAPI.Models
{
    public class Empresa
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("razaosocial")]
        [BsonRequired()]
        public string RazaoSocial { get; set; }

        [BsonElement("cnpj")]
        [BsonRequired()]
        public string CNPJ { get; set; }

        [BsonElement("diafechamento")]
        [BsonRequired()]
        public int DiaFechamento { get; set; }

        public Empresa(string razaoSocial, string cNPJ, int diaFechamento)
        {
            RazaoSocial = razaoSocial;
            CNPJ = cNPJ;
            DiaFechamento = diaFechamento;
        }
    }
}
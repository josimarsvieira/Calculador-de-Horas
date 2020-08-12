using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RestAPI.Models
{
    /// <summary>
    /// Classe responsavel pelos registros do Banco de Hora.
    /// </summary>
    public class BancoDeHoras
    {
        /// <summary>
        /// Id gerada pelo BD.
        /// </summary>
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// TimeSpan contendo as horas extras do funcionario.
        /// </summary>
        [BsonElement("horas_extras")]
        [BsonRequired()]
        public string HorasExtras { get; set; }

        /// <summary>
        /// String contendo a justificativa para adição.
        /// </summary>
        [BsonElement("justificativa")]
        [BsonRequired()]
        public string Justificativa { get; set; }

        /// <summary>
        /// Dia do registro.
        /// </summary>
        [BsonElement("data_registro")]
        [BsonRequired()]
        public DateTime DataRegistro { get; set; }

        [BsonElement("registro_funcionario")]
        [BsonRequired()]
        public int Funcionario { get; set; }

        public BancoDeHoras()
        {
        }
    }
}
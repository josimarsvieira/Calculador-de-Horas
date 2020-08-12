using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestAPI.Models
{
    /// <summary>
    /// Classe para registro de funcionario.
    /// </summary>
    public class Funcionario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Numero de registro.
        /// </summary>
        [BsonElement("registro")]
        [BsonRequired]
        public int Registro { get; set; }

        /// <summary>
        /// Nome do Funcionario.
        /// </summary>
        [BsonElement("nome")]
        [BsonRequired]
        public string Nome { get; set; }

        /// <summary>
        /// Função do funcionario.
        /// </summary>
        [BsonElement("funcao")]
        [BsonRequired]
        public string Funcao { get; set; }

        /// <summary>
        /// Hora de inicio do expediente.
        /// </summary>
        [BsonElement("hora_inicio")]
        [BsonRequired]
        public string HoraInicio { get; set; }

        /// <summary>
        /// Hora de termino do expediente.
        /// </summary>
        [BsonElement("hora_termino")]
        [BsonRequired]
        public string HoraTermino { get; set; }

        /// <summary>
        /// Hora de saida para o almoço.
        /// </summary>
        [BsonElement("hora_saida_almoco")]
        [BsonRequired]
        public string HoraSaidaAlmoco { get; set; }

        /// <summary>
        /// Hora de retorno do almoço.
        /// </summary>
        [BsonElement("hora_retorno_almoco")]
        [BsonRequired]
        public string HoraRetornoAlmoco { get; set; }

        /// <summary>
        /// Representa se é um fucionario ativo.
        /// </summary>
        [BsonElement("ativo")]
        [BsonRequired]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Ativo { get; set; }

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public Funcionario()
        {
        }
    }
}
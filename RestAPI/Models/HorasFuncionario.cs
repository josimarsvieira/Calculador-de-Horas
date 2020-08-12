using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models
{
    /// <summary>
    /// Classe para registro de horas do funcionario.
    /// </summary>
    public class HorasFuncionario
    {
        /// <summary>
        /// Id gerado pelo BD.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Hora de entrada no trabalho.
        /// </summary>
        [BsonElement("entrada")]
        [Required]
        public string Entrada { get; set; }

        /// <summary>
        /// Hora de saida do trabalho.
        /// </summary>
        [BsonElement("saida")]
        [Required]
        public string Saida { get; set; }

        /// <summary>
        /// Hora de saida para o Almoço
        /// </summary>
        [BsonElement("almoco_saida")]
        [Required]
        public string HoraSaidaAlmoco { get; set; }

        /// <summary>
        /// Hora de retorno do Almoço
        /// </summary>
        [BsonElement("almoco_retorno")]
        [Required]
        public string HoraRetornoAlmoco { get; set; }

        /// <summary>
        /// Horas extras feitas no dia.
        /// </summary>
        [BsonElement("extras")]
        [Required]
        public string Extras { get; set; }

        /// <summary>
        /// Id do funcionario vinculado ao registro.
        /// </summary>
        [BsonElement("registro_funcionario")]
        [Required]
        public int Funcionario { get; set; }

        /// <summary>
        /// Data do registro.
        /// </summary>
        [BsonElement("data_registro")]
        [Required]
        public DateTime DataRegistro { get; set; }

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public HorasFuncionario()
        {
        }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required, MinLength(5)]
        public string CC { get; set; }
        [Required ,MinLength(3)]
        public string Nombres { get; set; }
        [Required, MinLength(3)]
        public string Apellidos { get; set; }
        [Required, MinLength(3)]
        public string Direccion { get; set; }

    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlteredCarbon.Models
{
    public class Farmer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }
        [BsonRequired]
        public string name { get; set; }
        [BsonRequired]
        public string email { get; set; }
        [BsonRequired]
        public string phoneNumber { get; set; }
        [BsonRequired]
        public string password { get; set; }
        public decimal points { get; set; }
        public string accountType { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string> kitOrders { get; set; }


        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string> sellAgrWasteOrders { get; set; }
    }
}

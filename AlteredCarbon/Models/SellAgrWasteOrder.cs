using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlteredCarbon.Models
{
    public class SellAgrWasteOrder
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }
        public string type { get; set; }
        public decimal typePT { get; set; }
        public decimal typeAGRIW { get; set; }
        public decimal orderPoints { get; set; }
        public decimal acres { get; set; }
        public string status { get; set; }
        public DateTime datetimeOfOrder { get; set; }
        public DateTime datetimeOfApproval { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string farmer { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string approvedBy { get; set; }
    }
}

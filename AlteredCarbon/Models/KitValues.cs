using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlteredCarbon.Models
{
    public class KitValues
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string id { get; set; }
        public decimal INitrogen { get; set; }
        public decimal IPotassium { get; set; }
        public decimal IPhosphorus { get; set; }


        public decimal ICotton { get; set; }
        public decimal IRice { get; set; }
        public decimal IWheat { get; set; }
        public decimal ICorn { get; set; }



        public decimal ureaPOP { get; set; }
        public decimal superPhosphatePOP { get; set; }
        public decimal potassiumSulfatePOP { get; set; }
        public decimal biocharPOP { get; set; }


        public decimal wheatPT { get; set; }
        public decimal ricePT { get; set; }
        public decimal sunflowerPT { get; set; }
        public decimal barleyPT { get; set; }
        public decimal cottonPT { get; set; }
        public decimal cornPT { get; set; }



        public decimal riceAGRIW { get; set; }
        public decimal wheatAGRIW { get; set; }
        public decimal sunflowerAGRIW { get; set; }
        public decimal barleyAGRIW { get; set; }
        public decimal cottonAGRIW { get; set; }
        public decimal cornAGRIW { get; set; }


        public decimal multiplierN { get; set; }
        public decimal MultiplierP { get; set; }
        public decimal MultiplierK { get; set; }

    }
}

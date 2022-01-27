using AlteredCarbon.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AlteredCarbon.Database
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Farmer> _farmers;
        private readonly IMongoCollection<KitOrder> _kitorders;
        private readonly IMongoCollection<SellAgrWasteOrder> _sellOrders;
        private readonly IMongoCollection<KitValues> _kitValues;
        private readonly IMongoCollection<Cooperative> _cooperatives;

        public DbClient(IOptions<DbConfig> dbConfig)
        {
            var client = new MongoClient(dbConfig.Value.Connection_String);
            var database = client.GetDatabase(dbConfig.Value.Database_Name);
            _farmers = database.GetCollection<Farmer>("farmerusers");
            _kitorders = database.GetCollection<KitOrder>("kitorders");
            _sellOrders = database.GetCollection<SellAgrWasteOrder>("sellagrwasteorders");
            _kitValues = database.GetCollection<KitValues>("kits");
            _cooperatives = database.GetCollection<Cooperative>("cooperativeusers");
        }

        public IMongoCollection<Farmer> GetFarmersCollection() => _farmers;
        public IMongoCollection<Cooperative> GetCooperativesCollection() => _cooperatives;
        public IMongoCollection<KitOrder> GetKitOrderCollection() => _kitorders;

        public IMongoCollection<SellAgrWasteOrder> GetSellOrderCollection() => _sellOrders;
        public IMongoCollection<KitValues> GetKitValuesCollection() => _kitValues;
      
    }
}

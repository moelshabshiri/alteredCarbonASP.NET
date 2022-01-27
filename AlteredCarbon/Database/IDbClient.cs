using AlteredCarbon.Models;
using MongoDB.Driver;

namespace AlteredCarbon.Database
{
    public interface IDbClient
    {
        IMongoCollection<Farmer> GetFarmersCollection();
        IMongoCollection<KitOrder> GetKitOrderCollection();
        IMongoCollection<SellAgrWasteOrder> GetSellOrderCollection();
        IMongoCollection<KitValues> GetKitValuesCollection();
        IMongoCollection<Cooperative> GetCooperativesCollection();

    }
}

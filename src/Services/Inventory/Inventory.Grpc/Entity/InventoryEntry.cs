using Contract.Domain;
using Infrastructure.Extensions;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.Grpc.Entity;

[BsonCollection("InventoryEntries")]
public class InventoryEntry : MongoEntity
{
    [BsonElement("itemNo")]
    public string ItemNo { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}

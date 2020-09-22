using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;


    public class MongoDbRepository : IRepository
{
    private readonly IMongoCollection<Player> _playerCollection;
    private readonly IMongoCollection<BsonDocument> _bsonPlayerDocumentCollection;


    public MongoDbRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("game"); 
        _playerCollection = database.GetCollection<Player>("players");

        _bsonPlayerDocumentCollection = database.GetCollection<BsonDocument>("players");

    
    }
    public async Task<Player> Create(Player player)
    {
        player.Items = new List<Item>();
        await _playerCollection.InsertOneAsync(player);
        return player;
    }

    public async Task<Player> Delete(Guid id)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
        return await _playerCollection.FindOneAndDeleteAsync(filter);
    }

    public Task<Player> Get(Guid id)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
        return _playerCollection.Find(filter).FirstAsync();
        
    }

    public async Task<Player[]> GetAll()
    {
        var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
        return players.ToArray();
    }

    public async Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
        return null;
    }

    public async Task<Item> CreateItem(Guid playerId, Item item)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        var update = Builders<Player>.Update.Push<Item>(e=>e.Items,item);
        await _playerCollection.UpdateOneAsync(filter,update);
        return item;
        
    }
    public async Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player l = await _playerCollection.Find(filter).FirstAsync();
        return (from i in l.Items where i.Id == itemId select i).FirstOrDefault();
    }
    public async Task<Item[]> GetAllItems(Guid playerId)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
        Player p = await _playerCollection.Find(filter).FirstAsync();
        return p.Items.ToArray();
    }
    
    public async Task<Item> UpdateItem(Guid playerId, Item item)
    {
        //FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId) & Builders<Player>.Filter.AnyEq(x=>x.Items,item);
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId) 
        & Builders<Player>.Filter.ElemMatch(x=>x.Items,Builders<Item>.Filter.Eq(x=>x.Id,item.Id));
         //Player p = await _playerCollection.Find(filter).FirstAsync();
         var update = Builders<Player>.Update.Set(x=>x.Items[-1],item);
         await _playerCollection.UpdateOneAsync(filter,update);
        // var s = (from i in p.Items where i.Id == item.Id select i).FirstOrDefault();
        // var index = p.Items.IndexOf(s);
        // if(index!=-1)
        // p.Items[index] = item;
         return item;
    }
    public async Task<Item> DeleteItem(Guid playerId, Item item)
    {
        Console.WriteLine(item.Id);
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        //        &   Builders<Player>.Filter.ElemMatch(x=>x.Items,Builders<Item>.Filter.Eq(x=>x.Id,item.Id));
                //var update = Builders<Player>.Update.Pull("players",item);
        var update = Builders<Player>.Update.PullFilter("Items",Builders<Item>.Filter.Eq(x=>x.Id,item.Id)); //"Columns.$[].Values",

        UpdateResult result =await _playerCollection.UpdateOneAsync(filter,update);
        Console.WriteLine(result.ModifiedCount);
        //Player p = await _playerCollection.Find(filter).FirstAsync();
        //Console.WriteLine(p.Items[0].Id);
        return item;
    }
}



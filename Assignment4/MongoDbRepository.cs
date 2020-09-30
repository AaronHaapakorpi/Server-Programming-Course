using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;


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
        player.CreationTime = DateTime.Now;
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
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
         //Player p = await _playerCollection.Find(filter).FirstAsync();
         var update = Builders<Player>.Update.Set(x=>x.Score,player.Score);
         await _playerCollection.UpdateOneAsync(filter,update);
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

     public async Task<Player[]> GetWithScoreOver(int minScore)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Where(p=>p.Score>=minScore);

        var p = await _playerCollection.Find(filter).ToListAsync<Player>();
        return p.ToArray();
    }


    public async Task<Player> GetWithName(string name){
        var filter = Builders<Player>.Filter.Where(p=>p.Name.Equals(name));
        return await _playerCollection.Find(filter).FirstAsync();
    }

    public async Task<Player> UpdateName(Guid id, string name)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
        var update = Builders<Player>.Update.Set(x=>x.Name,name);
        await _playerCollection.UpdateOneAsync(filter,update);
     return null;
    }

    public async Task<Player[]> GetSorted(int limit)
    {
        var filter = Builders<Player>.Sort.Descending(p=>p.Score);
        return (await _playerCollection.Find(new BsonDocument()).Sort(filter).Limit(limit).ToListAsync()).ToArray();
    }

     public async Task<int> GetMostCommonLevel()
    {/*
         var levelCounts =
        _collection.Aggregate()
        .Project(p => p.Level)
        .Group(l => l, p => new LevelCount { Id = p.Key, Count = p.Sum() })
        .SortByDescending(l => l.Count)
        .Limit(3);
*/

        var result = await _playerCollection.AsQueryable()
        .GroupBy(l=>l.Level)
        .OrderByDescending(s=>s.Count())
        .Select(g=>g.Key).FirstOrDefaultAsync();
        return result;

    }
    public class LevelCount
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }

}



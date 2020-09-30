using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class FileRepository : IRepository
{
    public async Task<Player> Create(Player player)
    {
        if(!File.Exists("game-dev.txt"))File.Create("game-dev.txt");
        string old = await  File.ReadAllTextAsync("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(old);
        player.Id = Guid.NewGuid();
        player.CreationTime = DateTime.Now;
        (list ??= new List<Player>()).Add(player);
        File.WriteAllText("game-dev.txt",JsonConvert.SerializeObject(list));
        return null;
    }

    public async Task<Player> Delete(Guid id)
    {
        string old = await File.ReadAllTextAsync("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(old);
        Console.Write("id " + id);
        var playerToDelete = (from p in list where p.Id == id select p).FirstOrDefault();
        if(playerToDelete==null)return null;
        list.Remove(playerToDelete);
        File.WriteAllText("game-dev.txt",JsonConvert.SerializeObject(list));
        return playerToDelete;

    }

    public async Task<Player> Get(Guid id)
    {
        string text = await File.ReadAllTextAsync("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(text);
        return(from p in list where p.Id == id select p).FirstOrDefault();
        
    }

    public async Task<Player[]> GetAll()
    {
        string text = await File.ReadAllTextAsync("game-dev.txt");
        if(text.Length == 0) return null;
        return JsonConvert.DeserializeObject<Player[]>(text);
    }

    public async Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
       string text = await File.ReadAllTextAsync("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(text);
        Console.WriteLine(id + "   jaaaaaaa    " +player.Score);
        var playerToModify =(from p in list where p.Id == id select p).FirstOrDefault();
        if(playerToModify!=null){
            playerToModify.Score = player.Score;
        }
        else return null;
        Console.WriteLine(playerToModify.Score);
        File.WriteAllText("game-dev.txt",JsonConvert.SerializeObject(list));
        return playerToModify;
    }

    public  Task<Item> CreateItem(Guid playerId, Item item)
    {
        throw new NotImplementedException();
    }
    public Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        throw new NotImplementedException();
    }
    public  Task<Item[]> GetAllItems(Guid playerId)
    {
        throw new NotImplementedException();
    }
    public  Task<Item> UpdateItem(Guid playerId, Item item)
    {
        throw new NotImplementedException();
    }
    public  Task<Item> DeleteItem(Guid playerId, Item item)
    {
        throw new NotImplementedException();
    }

    public Task<Player[]> GetWithScoreOver(int minScore)
    {
        throw new NotImplementedException();
    }
    public Task<Player> GetWithName(string name){
        throw new NotImplementedException();;
    }

    public Task<Player> UpdateName(Guid id, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Player[]> GetSorted(int limit)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetMostCommonLevel()
    {
        throw new NotImplementedException();
    }
}
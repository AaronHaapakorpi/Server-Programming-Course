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
        string old = File.ReadAllText("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(old);
        player.Id = Guid.NewGuid();
        player.CreationTime = DateTime.Now;
        (list ??= new List<Player>()).Add(player);
        File.WriteAllText("game-dev.txt",JsonConvert.SerializeObject(list));
        return null;
    }

    public async Task<Player> Delete(Guid id)
    {
        string old = File.ReadAllText("game-dev.txt");
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
        string text = File.ReadAllText("game-dev.txt");
        var list = JsonConvert.DeserializeObject<List<Player>>(text);
        return(from p in list where p.Id == id select p).FirstOrDefault();
        
    }

    public async Task<Player[]> GetAll()
    {
        string text = File.ReadAllText("game-dev.txt");
        if(text.Length == 0) return null;
        return JsonConvert.DeserializeObject<Player[]>(text);
    }

    public async Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
       string text = File.ReadAllText("game-dev.txt");
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
}
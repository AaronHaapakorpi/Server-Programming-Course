using System;
using System.Threading.Tasks;

public interface IRepository
{
    Task<Player> Get(Guid id);
    Task<Player[]> GetAll();
    Task<Player> Create(Player player);
    Task<Player> Modify(Guid id, ModifiedPlayer player);
    Task<Player> Delete(Guid id);
    Task<Item> CreateItem(Guid playerId, Item item);
    Task<Item> GetItem(Guid playerId, Guid itemId);
    Task<Item[]> GetAllItems(Guid playerId);
    Task<Item> UpdateItem(Guid playerId, Item item);
    Task<Item> DeleteItem(Guid playerId, Item item);
    Task<Player[]> GetWithScoreOver(int minScore);
    Task<Player> GetWithName(string name);
    Task<Player> UpdateName(Guid id, string name);
    Task<Player[]> GetSorted(int limit);

    Task<int> GetMostCommonLevel();
}
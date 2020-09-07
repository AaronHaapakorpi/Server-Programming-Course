using System.Collections.Generic;
using System;
using System.Linq;
namespace Assignment2
{
    //static class for accessing methods that operate with items
    public static class PlayerItemOperations
    {
        public static Item[] GetItems(in Player player)
        {
            Item[] arr = new Item[player.Items.Capacity];
            for(int i=0; i< player.Items.Count;i++)
            {
                arr[i] = player.Items[i];
            }
            return arr;
        }

        public static Item[] GetItemsWithLinq(in Player player)
        {
            return player.Items.ToArray();
        }

        public static Item FirstItem(in Player player)
        {
            if(player.Items.Count==0)return null;
            return player.Items[0];
        }

         public static Item FirstItemWithLinq(in Player player)
        {
            if(player.Items.Count==0)return null;
            return player.Items.First();
        }

        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach(var item in player.Items)
            {
                process(item);
            }
        }
        public static void PrintItem(Item item){
            Console.WriteLine("id of item is " + item.Id);
            Console.WriteLine("level of item is " + item.Level);
        }

        public static Item RandomItem(){
            Item item = new Item();
            Random rnd = new Random(); //could pass as a parameter instead or something
            item.Id = Guid.NewGuid();
            item.Level = rnd.Next(1,9556);
            return item;
        }
    }
}
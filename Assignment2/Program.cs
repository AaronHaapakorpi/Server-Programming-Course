using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    class Program
    {
         
        //helper function for part 2
        public static Player Part2Test()
        {
            Player player = PlayerOperations.RandomPlayer();
            Item item = PlayerItemOperations.RandomItem();
            player.Items.Add(item);
            item = PlayerItemOperations.RandomItem();
            player.Items.Add(item);
            item = PlayerItemOperations.RandomItem();
            player.Items.Add(item);
            item = PlayerItemOperations.RandomItem();
            player.Items.Add(item);
            Console.WriteLine("highest level item was " + player.GetHighestLevelItem().Level);
            return player;
        }
        static void Main()
        {    //part 1 : Creates 1 mil players and prints if a duplicate guid was created (which won't happen anyway)
            Console.WriteLine("Part 1");
            Part1.RandomGuids(1000000);
            //part 2 : creates a player with a new id and random score and items with random levels and prints highest level item.
            Console.WriteLine("Part 2");
            Player player  =Part2Test();

           //part 3 Copies player item array to a new array and prints their ids. 
            Console.WriteLine("Part 3");
            Item[] arr = PlayerItemOperations.GetItems(player);

            foreach(var item in arr)Console.Write(item.Id);
            arr = PlayerItemOperations.GetItemsWithLinq(player);
            Console.WriteLine();
            foreach(var item in arr)Console.Write(item.Id);

            //part 4 Prints first item with [0] and linq First
            Console.WriteLine("\nPart 4");
            Console.WriteLine("first item " + PlayerItemOperations.FirstItem(player).Id);

            Console.WriteLine("first item linq " + PlayerItemOperations.FirstItemWithLinq(player).Id);

            //part 5 Calls delegate on each item, in this case prints the id and level of each item
            Console.WriteLine("Part 5");
            PlayerItemOperations.ProcessEachItem(player, PlayerItemOperations.PrintItem);

            //part 6 Same as part 5 with a lambda as the delegate
            Console.WriteLine("Part 6");
            /* Action<Item> PrintLambda = (Item item) => { //can assign lambda to a variable if needed
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Level);
            };*/
            PlayerItemOperations.ProcessEachItem(player,(Item item) => 
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Level);
            });

            //part 7 Populates a list of players with random scores and creates a new game with the list. Then it calls the game's method
            //that returns the top 10 scores in descending order and prints them.
            Console.WriteLine("Part 7");
            Console.WriteLine("Testing Game class with player");
            List<Player> playerList = new List<Player>();
            for(int i=0; i< 50000; i++)
            {
                playerList.Add(PlayerOperations.RandomPlayer());
            }
            Game<Player> game = new Game<Player>(playerList);

             var watch = new System.Diagnostics.Stopwatch();
             watch.Start();
            foreach(var topPlayer in game.GetTop10Players())
            {
                Console.WriteLine("score " + topPlayer.Score);
            }
             watch.Stop();
             Console.WriteLine("Execution Time of getting top 10 " +watch.ElapsedMilliseconds +" ms");

            //other playerTest
            Console.WriteLine("Testing a different game's player.");
            List<PlayerForAnotherGame> playerOtherList = new List<PlayerForAnotherGame>();
            for(int i=0; i< 50; i++)
            {
                playerOtherList.Add(PlayerOperations.RandomPlayerOtherGame());
            }
            Game<PlayerForAnotherGame> gameTwo = new Game<PlayerForAnotherGame>(playerOtherList);
            foreach(var topPlayer in gameTwo.GetTop10Players())
            {
                Console.WriteLine("score " + topPlayer.Score);
            }
        }
    }
}

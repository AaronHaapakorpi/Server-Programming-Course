using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2{
    //part 1
    //checking if values are unique is unneeded because the total number of unique keys is 2^128, chance of them not being unique is ridiculously low
    public static class Part1{
        public static void RandomGuids(int numberOfPlayers){
             var watch = new System.Diagnostics.Stopwatch();
             Player[] array = new Player[numberOfPlayers];
             watch.Start();
             HashSet<Guid> distinctTest = new HashSet<Guid>();
             //creating players and adding them to an array
             for(int i=0; i< numberOfPlayers; i++)
             {
                Player player = new Player();
                player.Id = Guid.NewGuid();
                array[i] = player;
             }
             //checking if we generated a distinct value by copying values to hashset
             bool check = true;
             for(int i=0; i < numberOfPlayers;i++)
             {
                 check = distinctTest.Add(array[i].Id); //hashset only accepts distinct values, so we check if a value got added
                 if (!check)break;
             }
            //var testSet = new HashSet<Guid> (array.Select(x => x.Id).ToHashSet()); //linq way, around 600ms slower on 10 mil
           // bool isUnique = array.Select(x=> x.Id).Distinct().Count() == array.Count(); //iterative linq way, also slower
             if(!check)Console.WriteLine("Found duplicate!");
             else Console.WriteLine("Didn't find duplicate.");

            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

    //alternate solution without newguid (while loops prevents duplicates though so checking is irrelevant)
    public static void RandomGuidsNoNewGuid(){
            Player[] array = new Player[1000000];
            HashSet<int> randomUsed = new HashSet<int>();
            int[] arrayCheck = new int[1000000];
            Random r = new Random();
            int toAdd = 1;
            Console.WriteLine("Setting guid values to random between 0 and 3591923");
            for(int i=0; i< 1000000; i++)
            {
                Player player = new Player();
                while(randomUsed.Count <i+1){
                    toAdd = r.Next(0,3591923); 
                    randomUsed.Add(toAdd);
                }
                arrayCheck[i] = toAdd;
                player.Id = ToGuid(toAdd);
                array[i] = player;
            } 
            if(arrayCheck.Length != arrayCheck.Distinct().Count()) 
            {
                Console.WriteLine("has duplicates");
            }
            else
            {
                Console.WriteLine("no duplicates");
            }
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
        
    }
}
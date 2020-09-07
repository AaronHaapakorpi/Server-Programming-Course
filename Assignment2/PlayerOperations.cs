using System.Collections.Generic;
using System;
namespace Assignment2
{
    public static class PlayerOperations{
         public static Player RandomPlayer(){
            Player player = new Player();
            player.Id = Guid.NewGuid();
            Random rnd = new Random();
            player.Score= rnd.Next(1,1000000000);
            player.Items = new List<Item>();
            return player;
        }

        public static PlayerForAnotherGame RandomPlayerOtherGame(){
            PlayerForAnotherGame player = new PlayerForAnotherGame();
            player.Id = Guid.NewGuid();
            Random rnd = new Random();
            player.Score= rnd.Next(1,100);
            player.Items = new List<Item>();
            return player;
        }
    }

}
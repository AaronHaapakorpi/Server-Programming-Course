using System;
using System.Collections.Generic;
namespace Assignment2
{
     public class PlayerForAnotherGame : IPlayer
    {
        public int SomeValue{get;set;}
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }
    }

}
using System.Collections.Generic;
using System;
namespace Assignment2
{

    public class Player : IPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }

    }
}
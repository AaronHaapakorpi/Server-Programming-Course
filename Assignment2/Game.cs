using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Collections.Specialized;

namespace Assignment2
{
public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players) {
            _players = players;
        }
        // topList =(from p in topList orderby p.Score descending select p).ToArray();

        //tested 3 implementations

        //#1 = linq query, sorts the list by player scores descending then selects up to 10 first players, takes ~ 250 ms for 5 million players

        //#2 = stores up to 10 first player scores in the top list and sorts it, iterates through players, compares score to top scores starting from worst score.
        //if a top score is larger than the player score, elements are shifted right until the largest top score that is smaller 
        //than the player score. takes ~ 35 ms for 5 million players

        //#3 = sorting a copy of the player list using a custom comparer to compare scores from the player objects and getting the first 10 from the sorted list, 
        //very slow, takes ~ 2905 ms for 5 million players
        public T[] GetTop10Players() 
        {
            // ... write code that returns 10 players with highest scores

            //#1
            /*
            return((from player in _players 
            orderby player.Score descending 
            select player)
            .Take(10).ToArray());*/

            //#2
            //makes a top list with a maximum player count of 10
            T[] topList = new T[Math.Clamp(_players.Count,0,10)];
            //sets maximum of 10 elements to top list
            for(int i =0; i<topList.Length; i++)topList[i] = _players[i];
            
           //sort elements so it's easy to compare player scores to top 10
           BubbleSort(ref topList);
            
            //holds the index where we found largest value
            int ind;
            //iterating through rest of the players
            for(int i = 10; i< _players.Count; i++)
            {
                //sets default index to 0 since if we go through the whole top list index 0 will be the largest
                ind = 0;
                //checking worst top score first, can stop comparing the moment the top list index score is larger than the player score, because it's ordered
                for(int t=9;t>=0;t--){
                    if (_players[i].Score < topList[t].Score)
                    {
                        ind = t+1;
                        break;
                    }
                }
                //shift elements if found a larger score, 10 = no larger score
                if(ind!=10){
                    //only shift values until index
                    for(int y=9;y>ind;y--)
                        {
                            topList[y]=topList[y-1];
                        }
                     topList[ind] = _players[i];
                }
            }
            return topList;

            //#3
            /*
            T[] topList = new T[Math.Clamp(_players.Count,0,10)];
            List<T> playerCopies = new List<T>(_players);
            playerCopies.Sort((s1, s2) => s2.Score.CompareTo(s1.Score)); 
            for(int i=0; i < topList.Length; i++){
                topList[i] = playerCopies[i];
            }
            return topList;
            */
        }

        //bubble sort is fine for a very small top 10 list
        public void BubbleSort(ref T[] t)
        {
            T temp;
            for(int it = 0; it < t.Length; it++){
                for(int i=0; i < t.Length-it-1; i++)
                {
                    if(t[i].Score < t[i+1].Score){
                        temp = t[i];
                        t[i] = t[i+1];
                        t[i+1]= temp;
                    }
                }
            } 
        }
    }
}
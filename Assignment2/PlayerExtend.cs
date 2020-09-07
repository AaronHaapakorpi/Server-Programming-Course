using System.Collections.Generic;
using System;
using System.Linq;
namespace Assignment2
{
    public static class PlayerExtend
    {
        //can use int index and int largestLevel if item contains a lot of unncessary information which would probably be faster, this is shorter though
         public static Item GetHighestLevelItem(this Player player)  
        {
            if(player.Items.Count==0)return null;

            //linq way
            /*return player.Items
            .Aggregate((agg, next) =>
            next.Level > agg.Level ? next : agg);*/

            //normal way
            Item largestItem = player.Items[0];
            for(int i=1; i< player.Items.Count; i++)
            {
                if(player.Items[i].Level > largestItem.Level)
                {
                    largestItem = player.Items[i];
                }
            }
            return largestItem;
        }
        
    }
}

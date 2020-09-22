using System;
using System.ComponentModel.DataAnnotations;
public class NewItem
{
     [Range(0,2)]
     public ItemType ItemType { get; set; }
     [Range(1,99)]
     public int Level{ get; set; }

     [PastDateValidation]
     public DateTime CreationDate { get; set; }
}


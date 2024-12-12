using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public string Description { get; set; }
    public Dictionary<string, float> Effect { get; set; }
    public int ID { get; set; }
    public string Locks { get; set; }
    public string Name { get; set; }
    public string PreReq { get; set; }
    public PriceValues Price { get; set; }
    public string Sprite { get; set; }

    public class PriceValues
    {
        public int Premium { get; set; }
        public int Standart { get; set; }
    }

}

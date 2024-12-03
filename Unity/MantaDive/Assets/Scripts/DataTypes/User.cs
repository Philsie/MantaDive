using System.Collections.Generic;

public class User
{
    public float DailyDepth { get; set; }
    public float MaxDepth { get; set; }
    public int Tier { get; set; }
    public int UUID { get; set; }
    public string UserName { get; set; }
    public Dictionary<string, float> Upgrades { get; set; }
}
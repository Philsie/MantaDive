using UnityEngine;

public class CurrencyResponse
{
    public Currencies Currency { get; set; }
    public int UUID { get; set; }
    public string UserName { get; set; }
}

public class Currencies
{
    public float Standard { get; set; }
    public float Premium { get; set; }
}

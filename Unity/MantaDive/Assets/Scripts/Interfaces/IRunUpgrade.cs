using System.Threading.Tasks;
using UnityEngine;
public interface IRunUpgrade
{
    void ApplyEffect();
    Task<bool> CanBeBought();
    void DeductCurrency();

    void TryPurchase();

}

using System.Collections;
using UnityEngine;

public class PremiumCurrencyCollectible : MonoBehaviour, ICollectible
{
    [SerializeField]
    private int value = 1;
    private bool isDone = false;
    public void FireEvent()
    {
        CollectiblesManager.ChangePremiumCurrencyByAmount(value);
        isDone = true;
    }

    public bool IsEventDone()
    {
        return isDone;
    }
    private IEnumerator SelfDestroyWhenDone()
    {
        yield return new WaitUntil(() => isDone);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDone)
        {
            StartCoroutine(SelfDestroyWhenDone());
            FireEvent();
        }
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PrimaryCurrencyCollectible : MonoBehaviour, ICollectible
{
    [SerializeField]
    private int value = 1;
    private bool isDone = false;
    public void FireEvent()
    {
        CollectiblesManager.ChangePrimaryCurrencyByAmount(value);
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
        Debug.Log("Trigger triggered");
        if (collision.gameObject.name.Equals("Player") && !isDone)
        {
            StartCoroutine(SelfDestroyWhenDone());
            FireEvent();
        }
    }
}

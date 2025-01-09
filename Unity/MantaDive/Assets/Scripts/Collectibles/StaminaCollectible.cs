using System.Collections;
using UnityEngine;

public class StaminaCollectible : MonoBehaviour, ICollectible
{
    private PlayerController playerController;
    [SerializeField]
    private int staminaValue = 10;
    private bool isDone = false;
    public void FireEvent()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(staminaValue);
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

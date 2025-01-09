using System.Collections;
using UnityEngine;

public class MagnetCollectible : MonoBehaviour, ICollectible
{
    private PlayerController playerController;
    [SerializeField]
    private float magnetStrength = 1.0f;
    [SerializeField]
    private float duration = 1.0f;
    private bool isDone = false;
    public void FireEvent()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.UpdateMagnetStrengthTemp(magnetStrength, duration);
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

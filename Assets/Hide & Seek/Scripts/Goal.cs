using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hider hider = collision.GetComponent<Hider>();
        if (hider != null) hider.ReachedGoal();
    }
}


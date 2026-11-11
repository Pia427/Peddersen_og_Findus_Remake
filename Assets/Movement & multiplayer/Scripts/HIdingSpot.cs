using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HidingSpot : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hider hider = other.GetComponentInParent<Hider>();
        if (hider != null) hider.EnterCover();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Hider hider = other.GetComponentInParent<Hider>();
        if (hider != null) hider.ExitCover();
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SlowZone : MonoBehaviour
{
    [Range(0.05f, 1f)]
    [SerializeField] private float speedMultiplier = 0.5f; // 0.5 = half speed

    private void Reset()
    {
        // Makes the collider a trigger automatically when the script is first added
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
        if (movement != null) movement.AddSlow(this, speedMultiplier);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
        if (movement != null) movement.RemoveSlow(this);
    }
}
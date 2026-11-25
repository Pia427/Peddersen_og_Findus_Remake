using UnityEngine;

public class FlipSpriteY : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    [Header("Mode")]
    [SerializeField] private bool followMovement = true;   // flip based on vertical movement
    [SerializeField] private bool spriteFacesUp = true;    // untick if your artwork faces down by default

    [Header("Auto flip (when followMovement is off)")]
    [SerializeField] private float flipInterval = 1f;      // seconds between flips

    private Rigidbody2D rb;
    private float timer;

    private void Awake()
    {
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (sprite == null) return;

        if (followMovement && rb != null)
        {
            float vy = rb.linearVelocity.y;
            if (Mathf.Abs(vy) > 0.1f)
            {
                bool movingUp = vy > 0f;
                sprite.flipY = spriteFacesUp ? !movingUp : movingUp;
            }
        }
        else if (!followMovement)
        {
            timer += Time.deltaTime;
            if (timer >= flipInterval)
            {
                timer = 0f;
                Flip();
            }
        }
    }

    // Can also be called from other scripts or UnityEvents
    public void Flip()
    {
        if (sprite != null) sprite.flipY = !sprite.flipY;
    }
}

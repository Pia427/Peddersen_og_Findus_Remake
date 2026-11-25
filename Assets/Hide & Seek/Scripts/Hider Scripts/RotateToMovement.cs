using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotateToMovement : MonoBehaviour
{
    [SerializeField] private Transform visual;              // the sprite object to rotate
    [SerializeField] private float rotationSpeed = 720f;    // degrees per second, 0 = instant
    [SerializeField] private float minSpeed = 0.1f;

    [Tooltip("Direction the artwork faces at rotation 0: Up = 90, Right = 0, Left = 180, Down = 270")]
    [SerializeField] private float spriteFacingAngle = 90f;

    [SerializeField] private bool snapTo8Directions = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (visual == null)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) visual = sr.transform;
        }
    }

    private void Update()
    {
        if (visual == null) return;

        Vector2 v = rb.linearVelocity;
        if (v.magnitude < minSpeed) return; // keep last facing when idle

        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (snapTo8Directions) angle = Mathf.Round(angle / 45f) * 45f;

        float target = angle - spriteFacingAngle;
        float current = visual.eulerAngles.z;

        float next = rotationSpeed <= 0f
            ? target
            : Mathf.MoveTowardsAngle(current, target, rotationSpeed * Time.deltaTime);

        visual.rotation = Quaternion.Euler(0f, 0f, next);
    }
}
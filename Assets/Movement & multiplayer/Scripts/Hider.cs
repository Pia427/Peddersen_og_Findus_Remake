using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hider : MonoBehaviour
{
    public static event Action<Hider> OnReachedGoal;

    [SerializeField] private Transform startPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color caughtColor = Color.red;
    [SerializeField] private float respawnDelay = 1f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private Color normalColor;

    // True while caught/respawning. The Seeker already ignores hiders with IsFound = true.
    public bool IsFound { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        if (sprite != null) normalColor = sprite.color;
    }

    private void Start() => MoveToStart();

    // Called by the Seeker when this hider is spotted
    public void Found()
    {
        if (IsFound) return;
        StartCoroutine(CaughtRoutine());
    }

    // Called by the Goal zone
    public void ReachedGoal()
    {
        if (IsFound) return;
        OnReachedGoal?.Invoke(this);
        MoveToStart(); // the "new hider" begins at the start
    }

    public void Freeze()
    {
        rb.linearVelocity = Vector2.zero;
        if (movement != null) movement.enabled = false;
    }

    private IEnumerator CaughtRoutine()
    {
        IsFound = true;
        Freeze();
        if (sprite != null) sprite.color = caughtColor;

        yield return new WaitForSeconds(respawnDelay);

        MoveToStart();
        if (sprite != null) sprite.color = normalColor;
        if (movement != null) movement.enabled = true;
        IsFound = false;
    }

    private void MoveToStart()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = startPoint.position;
        transform.position = startPoint.position;
    }
}


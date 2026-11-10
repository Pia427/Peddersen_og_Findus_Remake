using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hider : MonoBehaviour
{
    public static event Action<Hider> OnReachedGoal;
    public static event Action<Hider> OnCaught;

    [SerializeField] private Transform startPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color caughtColor = Color.red;
    [SerializeField] private float respawnDelay = 1f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private Color normalColor;
    private bool locked; // set when the game is over

    public bool IsFound { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        if (sprite != null) normalColor = sprite.color;
    }

    private void Start() => MoveToStart();

    public void Found()
    {
        if (IsFound || locked) return;
        StartCoroutine(CaughtRoutine());
    }

    public void ReachedGoal()
    {
        if (IsFound || locked) return;
        OnReachedGoal?.Invoke(this);
        if (!locked) MoveToStart();
    }

    // Called by the manager when the game ends. Nobody can move or respawn after this.
    public void Lock()
    {
        locked = true;
        StopAllCoroutines();
        Freeze();
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

        OnCaught?.Invoke(this); // the manager may lock us if this was the last life
        if (locked) yield break;

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
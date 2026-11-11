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
    [Range(0.1f, 1f)]
    [SerializeField] private float hiddenAlpha = 0.5f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private Color normalColor;
    private bool locked;
    private int coverCount; // how many hiding spots we're currently inside

    public bool IsFound { get; private set; }
    public bool IsHidden => coverCount > 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        if (sprite != null) normalColor = sprite.color;
    }

    private void Start() => MoveToStart();

    public void EnterCover()
    {
        coverCount++;
        UpdateVisual();
    }

    public void ExitCover()
    {
        coverCount = Mathf.Max(0, coverCount - 1);
        UpdateVisual();
    }

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
        UpdateVisual();

        OnCaught?.Invoke(this);
        if (locked) yield break;

        yield return new WaitForSeconds(respawnDelay);

        MoveToStart();
        if (movement != null) movement.enabled = true;
        IsFound = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (sprite == null) return;

        Color c = IsFound ? caughtColor : normalColor;
        if (IsHidden && !IsFound) c.a = hiddenAlpha;
        sprite.color = c;
    }

    private void MoveToStart()
    {
        coverCount = 0; // fresh start; entering a spot again re-registers
        rb.linearVelocity = Vector2.zero;
        rb.position = startPoint.position;
        transform.position = startPoint.position;
        UpdateVisual();
    }
}
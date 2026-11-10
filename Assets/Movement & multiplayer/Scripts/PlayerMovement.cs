using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public enum ControlScheme { WASD, Arrows }

    [SerializeField] private ControlScheme controls = ControlScheme.WASD;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Each active slow source and its multiplier (e.g. 0.5 = half speed)
    private readonly Dictionary<object, float> slowEffects = new Dictionary<object, float>();

    public void AddSlow(object source, float multiplier) => slowEffects[source] = multiplier;
    public void RemoveSlow(object source) => slowEffects.Remove(source);

    private float SpeedMultiplier
    {
        get
        {
            float result = 1f;
            foreach (float m in slowEffects.Values)
                result = Mathf.Min(result, m); // strongest slow wins
            return result;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f;
        float y = 0f;

        if (controls == ControlScheme.WASD)
        {
            if (kb.aKey.isPressed) x -= 1f;
            if (kb.dKey.isPressed) x += 1f;
            if (kb.sKey.isPressed) y -= 1f;
            if (kb.wKey.isPressed) y += 1f;
        }
        else
        {
            if (kb.leftArrowKey.isPressed) x -= 1f;
            if (kb.rightArrowKey.isPressed) x += 1f;
            if (kb.downArrowKey.isPressed) y -= 1f;
            if (kb.upArrowKey.isPressed) y += 1f;
        }

        moveInput = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed * SpeedMultiplier;
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class FootstepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepClips;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.6f;
    [SerializeField] private float stepInterval = 0.4f;   // seconds between steps at normal speed
    [SerializeField] private float normalSpeed = 5f;      // match PlayerMovement's Move Speed
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private AudioSource source;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    private void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        bool moving = movement != null && movement.enabled && speed > minSpeed;

        if (!moving)
        {
            timer = 0f; // so the first step plays right when they start moving
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        PlayStep();

        // Slower movement (e.g. in a slow zone) means slower steps
        timer = stepInterval * (normalSpeed / speed);
    }

    private void PlayStep()
    {
        if (stepClips == null || stepClips.Length == 0) return;

        AudioClip clip = stepClips[Random.Range(0, stepClips.Length)];
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.PlayOneShot(clip, volume);
    }
}
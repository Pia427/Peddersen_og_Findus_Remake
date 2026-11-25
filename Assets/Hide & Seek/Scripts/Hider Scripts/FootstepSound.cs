using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class FootstepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepClips;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.6f;
    [SerializeField] private float stepInterval = 0.4f;
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
        source.volume = volume;
    }

    private void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        bool moving = movement != null && movement.enabled && speed > minSpeed;

        if (!moving)
        {
            timer = 0f;
            if (source.isPlaying) source.Stop(); // cut the sound immediately
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        PlayStep();
        timer = stepInterval * (normalSpeed / speed);
    }

    private void PlayStep()
    {
        if (stepClips == null || stepClips.Length == 0) return;

        source.clip = stepClips[Random.Range(0, stepClips.Length)];
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.Play(); // replaces any previous step and can be stopped
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]
public class Goal : MonoBehaviour
{
    [SerializeField] private AudioClip goalClip;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.8f;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f; // 2D, same volume anywhere on the map
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hider hider = other.GetComponentInParent<Hider>();
        if (hider == null) return;

        if (!(hider.ReachedGoal() && goalClip == null))
            source.PlayOneShot(goalClip, volume);
    }
}
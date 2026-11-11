using UnityEngine;

public class Seeker : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource radarSource;
    [SerializeField] private AudioClip pingClip;
    [Range(0f, 1f)]
    [SerializeField] private float pingVolume = 0.5f;
    [SerializeField] private int pingsPerRotation = 1;   // 1 = one ping per full 360°
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 60f; // degrees per second, positive = clockwise

    [Header("Vision")]
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private LayerMask hiderMask;
    [SerializeField] private LayerMask obstacleMask;

    public float ViewAngle => viewAngle;
    public float ViewDistance => viewDistance;
    public LayerMask ObstacleMask => obstacleMask;
    private float sweptSinceLastPing;
    private float pingAngle => 360f / Mathf.Max(1, pingsPerRotation);

    private bool isActive = true;

    public void SetActive(bool value)
    {
        {
        isActive = value;

        if (!value && radarSource != null) radarSource.Stop(); // cut off a ping mid-play
        } 
    } 

    private void Start()
    {
        PlayPing();
    }
        
    private void Update()
    {
        if (!isActive) return;

        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        DetectHiders();

        {
            if (!isActive) return;

            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, -step);

            TrackSweepForPing(Mathf.Abs(step));
            DetectHiders();
        }
    }

    private void TrackSweepForPing(float stepDegrees)
{
    sweptSinceLastPing += stepDegrees;

    if (sweptSinceLastPing >= pingAngle)
    {
        sweptSinceLastPing -= pingAngle;
        PlayPing();
    }
}

private void PlayPing()
{
    if (radarSource == null || pingClip == null) return;
    radarSource.PlayOneShot(pingClip, pingVolume);
}

    private void DetectHiders()
    {
        Vector2 origin = transform.position;
        Collider2D[] candidates = Physics2D.OverlapCircleAll(origin, viewDistance, hiderMask);

        foreach (Collider2D col in candidates)
        {
            if (!col.TryGetComponent(out Hider hider) || hider.IsFound) continue;
            if (hider.IsHidden) continue;

            Vector2 toHider = (Vector2)col.transform.position - origin;

            // Inside the cone?
            if (Vector2.Angle(transform.up, toHider) > viewAngle * 0.5f) continue;

            // Blocked by a wall?
            if (Physics2D.Raycast(origin, toHider.normalized, toHider.magnitude, obstacleMask)) continue;

            hider.Found();
        }
    }
}
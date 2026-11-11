using UnityEngine;

public class Seeker : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource radarSource;   
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

    private bool isActive = true;

    public void SetActive(bool value)
    {
       {
    isActive = value;

    if (radarSource == null) return;
    if (value) radarSource.Play();
    else radarSource.Stop();
} 
    } 
        
    private void Update()
    {
        if (!isActive) return;

        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        DetectHiders();
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
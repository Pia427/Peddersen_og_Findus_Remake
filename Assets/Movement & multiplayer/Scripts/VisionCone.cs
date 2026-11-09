using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private int rayCount = 40;

    private Seeker seeker;
    private Mesh mesh;

    private void Awake()
    {
        seeker = GetComponentInParent<Seeker>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sortingOrder = 1;
    }

    private void LateUpdate()
    {
        float angle = seeker.ViewAngle;
        float half = angle * 0.5f;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];
        vertices[0] = Vector3.zero;

        for (int i = 0; i <= rayCount; i++)
        {
            float a = -half + angle * i / rayCount;
            Vector3 localDir = Quaternion.Euler(0f, 0f, a) * Vector3.up;
            Vector3 worldDir = transform.TransformDirection(localDir);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, worldDir, seeker.ViewDistance, seeker.ObstacleMask);
            float dist = hit.collider != null ? hit.distance : seeker.ViewDistance;

            vertices[i + 1] = localDir * dist;

            if (i < rayCount)
            {
                int t = i * 3;
                triangles[t] = 0;
                triangles[t + 1] = i + 2;
                triangles[t + 2] = i + 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
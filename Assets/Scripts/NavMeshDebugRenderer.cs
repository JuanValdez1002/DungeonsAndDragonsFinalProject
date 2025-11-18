using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class NavMeshDebugRenderer : MonoBehaviour
{
    public Color meshColor = new Color(0f, 0.5f, 1f, 0.4f);
    public bool drawEdges = true;
    public bool drawTriangles = true;

    private void OnDrawGizmos()
    {
        var navMeshData = NavMesh.CalculateTriangulation();

        if (navMeshData.vertices == null || navMeshData.vertices.Length == 0)
            return;

        Gizmos.color = meshColor;

        // Draw triangles
        if (drawTriangles)
        {
            for (int i = 0; i < navMeshData.indices.Length; i += 3)
            {
                Vector3 v1 = navMeshData.vertices[navMeshData.indices[i]];
                Vector3 v2 = navMeshData.vertices[navMeshData.indices[i + 1]];
                Vector3 v3 = navMeshData.vertices[navMeshData.indices[i + 2]];

                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v3);
                Gizmos.DrawLine(v3, v1);
            }
        }

        // Draw outer edges thicker
        if (drawEdges)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < navMeshData.indices.Length; i += 3)
            {
                Vector3 v1 = navMeshData.vertices[navMeshData.indices[i]];
                Vector3 v2 = navMeshData.vertices[navMeshData.indices[i + 1]];
                Vector3 v3 = navMeshData.vertices[navMeshData.indices[i + 2]];

                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v3);
                Gizmos.DrawLine(v3, v1);
            }
        }
    }
}

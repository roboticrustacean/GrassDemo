using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlade : MonoBehaviour
{
    public Transform bottomLeft; // Control point 1
    public Transform top;        // Control point 2 (top)
    public Transform bottomRight; // Control point 3

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        CreateGrassBladeMesh();

        // Assign the mesh to the MeshFilter here if you want to visualize it in the editor
        if (GetComponent<MeshFilter>() != null)
        {
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    public void CreateGrassBladeMesh()
    {
        mesh = new Mesh();

        // Check if control points are assigned
        if (bottomLeft == null || top == null || bottomRight == null)
        {
            Debug.LogError("One or more control points are not assigned.");
            return; // Exit if any control point is null
        }

        // Sample points along the quadratic Bézier curve (only drawing 1/3 of the curve)
        int sampleCount = 10; // Number of points along the curve
        Vector3[] vertices = new Vector3[sampleCount * 2]; // 2 vertices per sample point (left and right)
        Vector3[] normals = new Vector3[sampleCount]; // Normals for each vertex

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)(sampleCount - 1) * (1f / 3f); // Adjust to only sample 1/3 of the curve
            Vector3 point = CalculateQuadraticBezierPoint(t, bottomLeft.position, top.position, bottomRight.position);
            vertices[i] = point; // Middle vertex

            // Calculate normals for sideways offset
            normals[i] = CalculateNormal(t);
            vertices[i + sampleCount] = point + normals[i] * 0.05f; // Offset right side
        }

        // Create triangles connecting the points
        int[] triangles = new int[(sampleCount - 1) * 6]; // Two triangles per segment
        for (int i = 0; i < sampleCount - 1; i++)
        {
            triangles[i * 6] = i; // Left vertex
            triangles[i * 6 + 1] = i + 1; // Right vertex
            triangles[i * 6 + 2] = i + sampleCount; // Upper left vertex

            triangles[i * 6 + 3] = i + sampleCount; // Upper left vertex
            triangles[i * 6 + 4] = i + 1; // Right vertex
            triangles[i * 6 + 5] = i + 1 + sampleCount; // Upper right vertex
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }

    Vector3 CalculateNormal(float t)
    {
        Vector3 p0 = bottomLeft.position;
        Vector3 p1 = top.position;
        Vector3 p2 = bottomRight.position;

        // Calculate derivative to get the normal
        Vector3 derivative = 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
        Vector3 normal = Vector3.Cross(derivative, Vector3.up).normalized; // Cross with up vector for sideways direction
        return normal;
    }
}
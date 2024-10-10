using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrassRenderer : MonoBehaviour
{
    public Material grassMaterial; // Material to apply to grass blades
    public int grassBladeCount = 1024;

    // Adjust this value to scale the size of the grass blades
    public Vector3 grassBladeScale = new Vector3(0.5f, 3f, 0.5f); // Taller and wider grass blades

    private List<GrassBlade> grassBlades = new List<GrassBlade>();

    void Start()
    {
        for (int i = 0; i < grassBladeCount; i++)
        {
            // Create a new GameObject for each grass blade
            GameObject grassBladeObject = new GameObject($"GrassBlade_{i}");

            // Add MeshFilter and MeshRenderer components to the grass blade object
            MeshFilter meshFilter = grassBladeObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = grassBladeObject.AddComponent<MeshRenderer>();
            meshRenderer.material = grassMaterial; // Assign the grass material

            // Set position of the grass blade within a defined area
            grassBladeObject.transform.position = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));

            // Add a random rotation around the Y-axis
            grassBladeObject.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            // Add the GrassBlade component
            GrassBlade grassBlade = grassBladeObject.AddComponent<GrassBlade>();

            // Create control points for the Bézier curve
            GameObject bottomLeft = new GameObject("BottomLeft");
            GameObject top = new GameObject("Top");
            GameObject bottomRight = new GameObject("BottomRight");

            // Set positions for the control points
            bottomLeft.transform.position = new Vector3(0, 0, 0);
            top.transform.position = new Vector3(0, 1, 0);
            bottomRight.transform.position = new Vector3(0, 0, 1);

            // Set the control points as children of the grass blade object
            bottomLeft.transform.parent = grassBladeObject.transform;
            top.transform.parent = grassBladeObject.transform;
            bottomRight.transform.parent = grassBladeObject.transform;

            // Assign control points to the GrassBlade component
            grassBlade.bottomLeft = bottomLeft.transform;
            grassBlade.top = top.transform;
            grassBlade.bottomRight = bottomRight.transform;

            // Create the mesh for the grass blade
            grassBlade.CreateGrassBladeMesh(); // Ensure this is called after assigning control points

            // Assign the generated mesh to the MeshFilter
            meshFilter.mesh = grassBlade.GetComponent<MeshFilter>().mesh;

            // Adjust the scale of the grass blade
            grassBladeObject.transform.localScale = grassBladeScale;

            // Store reference to the grass blade
            grassBlades.Add(grassBlade);
        }
    }

    void Update()
    {
        // Draw all the grass blades using instancing
        foreach (GrassBlade grassBlade in grassBlades)
        {
            if (grassBlade.GetComponent<MeshFilter>() != null) // Check if MeshFilter exists
            {
                Graphics.DrawMesh(grassBlade.GetComponent<MeshFilter>().mesh, grassBlade.transform.localToWorldMatrix, grassMaterial, 0);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class handle_json_file : MonoBehaviour
{
    [System.Serializable]
    public class Country
    {
        public string name;
        public List<List<List<float>>> coordinates; // List of [x, y] points
    }

    [System.Serializable]
    public class CountryData
    {
        public List<Country> countries;
    }

    public string jsonFilePath = "Assets/Data/simplified_coordinates_1.json";
    public Material countryMaterial;

    void Start()
    {
        // Clear existing children in CountryManager
        
        // Load and parse the JSON file
        string json = File.ReadAllText(jsonFilePath);
        List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(json);

        // Generate meshes for each country
        foreach (Country country in countries)
        {
            CreateCountryMesh(country);
        }
    }
    void CreateCountryMesh(Country country)
    {
        GameObject countryObject = new GameObject(country.name);
        countryObject.transform.parent = transform;

        float scale = 0.05f;

        foreach (var polygon in country.coordinates)
        {
            // Create Mesh for each polygon
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // Add vertices for the current polygon
            for (int i = 0; i < polygon.Count; i++)
            {
                float x = polygon[i][0]; // Longitude
                float y = polygon[i][1]; // Latitude
                vertices.Add(new Vector3(x * scale, y * scale, 0)); // XY plane
            }

            // Log vertex count
            // Debug.Log($"Country: {country.name}, Polygon Vertex Count: {vertices.Count}");

            if (vertices.Count < 3)
            {
                // Debug.LogWarning($"Country {country.name} has a polygon with fewer than 3 vertices. Skipping.");
                continue; // Skip invalid polygons
            }

            // Generate triangles (simple fan triangulation for convex polygons)
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateNormals();


            // Assign Mesh and Material
            GameObject polygonObject = new GameObject($"{country.name}_Polygon");
            polygonObject.transform.parent = countryObject.transform;
            MeshFilter meshFilter = polygonObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = polygonObject.AddComponent<MeshRenderer>();
            
            meshRenderer.material = countryMaterial;

            // Add Collider for Interaction
            MeshCollider meshCollider = polygonObject.AddComponent<MeshCollider>();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh; // Assign the generated mesh

            if (meshCollider.sharedMesh == null)
            {
                Debug.LogError($"MeshCollider.sharedMesh is null for {polygonObject.name}");
            }
            else
            {
                Debug.Log($"MeshCollider successfully assigned for {polygonObject.name}");
            }


            // Add Collider for Interaction
            if (!polygonObject.GetComponent<CountrySelector>())
            {
                polygonObject.AddComponent<CountrySelector>();
            }
        }
    }
}

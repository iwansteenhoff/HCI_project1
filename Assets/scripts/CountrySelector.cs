using UnityEngine;

public class CountrySelector : MonoBehaviour
{
    private Material[] originalMaterials;
    public Material highlightMaterial; // Material for highlighting the selected country

    private Transform parentCountry;

    void Start()
    {
        parentCountry = transform.parent;
        // Assign the highlight material programmatically
        highlightMaterial = Resources.Load<Material>("Materials/selectioncountry");
        if (highlightMaterial == null)
        {
            Debug.LogError("HighlightMaterial not found. Ensure it exists in a 'Resources/Materials' folder.");
        }
    }

    void OnMouseEnter()
    {
        // Highlight all child meshes
        MeshRenderer[] renderers = parentCountry.GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = highlightMaterial;
        }
    }

    void OnMouseExit()
    {
        // Revert all child meshes to their original material
        MeshRenderer[] renderers = parentCountry.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    void OnMouseDown()
    {
        // Log the country name when it's clicked
        Debug.Log($"Selected Country: {gameObject.name}");
    }
}

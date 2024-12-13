using UnityEngine;
using UnityEngine.EventSystems;

public class BlockMeshInteraction : MonoBehaviour
{
    public GameObject[] meshes; // Assign your mesh objects here

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            foreach (GameObject mesh in meshes)
            {
                mesh.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        }
        else
        {
            foreach (GameObject mesh in meshes)
            {
                mesh.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
using UnityEngine;

public class DragAndZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float dragSpeed = 1f;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    private void Update()
    {
        // Zoom
        float zoomAmount = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize += zoomAmount;

        // Clamp zoom value if needed
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2f, 10f);

        // Drag
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentMousePosition;
            transform.position += difference * dragSpeed;
            dragOrigin = currentMousePosition;
        }
    }
}
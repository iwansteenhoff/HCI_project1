using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomingandscrolling1 : MonoBehaviour
{
    private Vector3 dragOrigin;
    public float zoomSpeed = 10f;
    public float minZoom = 0.05f;
    public float maxZoom = 100f;

    public float mapWidth = 1000f;   // Map width in world units
    public float mapHeight = 1000f;  // Map height in world units

    public MenuArea MenuArea;
    private TimelineSlider timelineSlider; // Reference to TimelineSlider

    void Start()
    {
        // Find the TimelineSlider in the scene
        timelineSlider = FindObjectOfType<TimelineSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable map controls if the slider is active
        if (timelineSlider != null && timelineSlider.isSliderActive) return;

        // Only process input if the mouse is NOT over the panel
        if (MenuArea != null && MenuArea.isMouseOverMenu) return;

        // On Mouse Down or Touch Start, store drag start point
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // On Mouse Drag or Touch Move, update the map position
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference;
        }

        // For desktop: Mouse Scrollwheel Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll);

        // Clamp the camera position after zooming
        ClampCameraPosition();
    }

    // Zoom around the mouse position
    void Zoom(float scroll)
    {
        // Get the current mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // We don't want to change the z-axis, only 2D space

        // Adjust the camera orthographic size
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        // Get the new camera position
        Vector3 offset = mouseWorldPos - Camera.main.transform.position;

        // Update the camera's position based on the zoom level and the mouse position
        Camera.main.transform.position += offset * scroll * zoomSpeed;
    }

    // Method to keep the camera within map boundaries
    void ClampCameraPosition()
    {
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float minX = -mapWidth / 2 + horzExtent;
        float maxX = mapWidth / 2 - horzExtent;
        float minY = -mapHeight / 2 + vertExtent;
        float maxY = mapHeight / 2 - vertExtent;

        Vector3 clampedPosition = Camera.main.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        Camera.main.transform.position = clampedPosition;
    }
}


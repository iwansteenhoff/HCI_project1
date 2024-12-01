using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomingandscrolling : MonoBehaviour
{
    private Vector3 dragOrigin;
    public float zoomSpeed = 10f;
    public float minZoom = 0.05f;
    public float maxZoom = 100f;

    public float mapWidth = 1000f;   // Map width in world units
    public float mapHeight = 1000f;  // Map height in world units

    public MenuArea MenuArea;

    // Update is called once per frame
    void Update()
    {
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
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);


        // Clamp the camera position after zooming
        ClampCameraPosition();
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

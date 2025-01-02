using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experimentselector : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.tag == "Not Selected")
                {
                    // Change the country's tag and refresh its appearance
                    hit.collider.gameObject.tag = "Selected";
                    GetComponent<MapStyleController>().UpdateSettings();
                }
            }
        
    }
}

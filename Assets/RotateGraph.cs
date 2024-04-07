using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGraph : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public Camera cam;
    public Vector3 rotationCenter = new Vector3(-50f, 72f, -450f);

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;

            cam.transform.RotateAround(rotationCenter, cam.transform.right, rotX);
            cam.transform.RotateAround(rotationCenter, Vector3.up, rotY);

            lastMousePosition = Input.mousePosition;
        }

        // Arrow Key Input (Optional)
        float arrowKeyHorizontal = Input.GetAxis("Horizontal");
        float arrowKeyVertical = Input.GetAxis("Vertical");

        if (arrowKeyHorizontal != 0 || arrowKeyVertical != 0)
        {
            float rotX = arrowKeyVertical * rotationSpeed * Time.deltaTime;
            float rotY = -arrowKeyHorizontal * rotationSpeed * Time.deltaTime;

            cam.transform.RotateAround(rotationCenter, cam.transform.right, rotX);
            cam.transform.RotateAround(rotationCenter, Vector3.up, rotY);

            Debug.Log("Arrow Key Movement: Horizontal = " + arrowKeyHorizontal + ", Vertical = " + arrowKeyVertical);
        }
    }
}

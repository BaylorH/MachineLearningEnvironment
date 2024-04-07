using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGraph : MonoBehaviour
{
    public float PCRotationSpeed = 10f;
    public Camera cam;

    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * PCRotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * PCRotationSpeed;

        Vector3 right = Vector3.Cross(lhs: cam.transform.up, rhs: transform.position - cam.transform.position);
        Vector3 up = Vector3.Cross(lhs: transform.position - cam.transform.position, rhs: right);
        transform.rotation = Quaternion.AngleAxis(-rotX, up) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(rotY, right) * transform.rotation;
    }

    private void Update()
    {
        // get user input
        foreach(Touch touch in Input.touches)
        {
            Debug.Log(message: "Touching at: " + touch.position);
            Ray camRay = cam.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(camRay, out raycastHit, maxDistance: 10))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log(message: "Touch phase began at: " + touch.position);
                } else if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log(message: "Touch phase Moved");
                    transform.Rotate(xAngle: touch.deltaPosition.y * PCRotationSpeed,
                        yAngle: -touch.deltaPosition.x * PCRotationSpeed, zAngle: 0, relativeTo: Space.World);
                } 
                else if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log(message: "Touch phase Ended");
                }
            }
        }
    }
}

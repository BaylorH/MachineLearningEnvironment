using UnityEngine;

public class OrbitCamera : MonoBehaviour {
    public Transform target; // The target object to orbit around
    public float distance = 5.0f; // Distance from the target object
    public float xSpeed = 120.0f; // Rotation speed around the x axis
    public float ySpeed = 120.0f; // Rotation speed around the y axis

    private float x = 0.0f; // x angle
    private float y = 0.0f; // y angle

    void Start() {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void LateUpdate() {
        if (target) {
            if (Input.GetMouseButton(0)) // Check if the left mouse button is held down
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}

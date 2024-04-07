using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform target_pos;
    public float speed = 1; //speed variable is inverse with smoothDamp. raise the variable and object moves slower
    public Vector3 velocity = Vector3.zero;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target_pos.position, ref velocity, speed * Time.deltaTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Vector3 target_pos = new Vector3(0f,0f,0f);
    public float speed = 1;
    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void moveTowards(Vector3 position)
    {
        // Specify the target position
        target_pos = position; // Example target position

    }

    // Update is called once per frame
    void Update()
    {
        if (!(target_pos.x == 0f && target_pos.y == 0f && target_pos.z == 0f))
        {
            transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref velocity, speed * Time.deltaTime);
        }
    }
}

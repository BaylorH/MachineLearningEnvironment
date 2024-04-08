using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Vector3 _target = Vector3.zero;
    [SerializeField]
    private float
        _distance = 10f,
        _xSpeed = 250f,
        _ySpeed = 120f,
        _zoomSpeed = 90f,
        _yMinLimit = -20f,
        _yMaxLimit = 80f;

    private float _x, _y;
    private Vector3 _startPos;

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void Start()
    {
        _startPos = _target;
        var eulerAngles = transform.eulerAngles;
        _x = eulerAngles.x; //TODO fix maybe
        _y = eulerAngles.y;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            var axis = Input.GetAxis("Mouse X");
            var axis2 = Input.GetAxis("Mouse Y");
            _x += axis * _xSpeed * 0.03f;
            _y += axis2 * _ySpeed * 0.03f;
            _y = ClampAngle(_y, _yMinLimit, _yMaxLimit);
        }
        else if (Input.GetMouseButton(1))
        {
            var axis = Input.GetAxis("Mouse X");
            var axis2 = Input.GetAxis("Mouse Y");
            var num = (Mathf.Abs(axis) <= Mathf.Abs(axis2)) ? axis2 : axis;
            num = -num * _zoomSpeed * 0.03f;
            _distance += num * (Mathf.Max(_distance, 0.02f) * 0.03f);
        }
        var rotation = Quaternion.Euler(_y, _x, 0f);
        var position = rotation * new Vector3(0f, 0f, -_distance) + _target;
        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }
}
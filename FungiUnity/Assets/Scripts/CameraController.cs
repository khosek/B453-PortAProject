using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 10.0f;
    public float xSpeed = 120.0f;

    private float x = 0.0f;
    private Vector3 initialOffset;

    void Start()
    {
        if (!target)
        {
            Debug.LogWarning("No target set for OrbitCamera script. The camera will not follow an object.");
            return;
        }

        initialOffset = transform.position - target.position;
        distance = initialOffset.magnitude;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
    }

    void LateUpdate()
    {
        if (target && Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

            Quaternion rotation = Quaternion.Euler(0, x, 0);
            Vector3 position = rotation * initialOffset + target.position;

            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.position = position;
        }
    }
}

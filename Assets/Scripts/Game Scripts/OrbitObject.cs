using System;
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    [Tooltip("The transform to orbit around.")]
    public Transform target;
    
    [Tooltip("How fast the object orbits (degrees per second).")]
    public float orbitSpeed = 15f;
    
    [Tooltip("The axis to orbit around (Vector3.up is the Y axis).")]
    public Vector3 orbitAxis = Vector3.up;
    
    [Tooltip("Check this to make the object always face the target while orbiting.")]
    public bool lookAtTarget = true;

    private void Awake()
    {
        target = transform;
    }

    void Update()
    {
        if (target != null)
        {
            transform.RotateAround(target.position, orbitAxis, orbitSpeed * Time.deltaTime);

            if (lookAtTarget)
            {
                transform.LookAt(target);
            }
        }
    }
}
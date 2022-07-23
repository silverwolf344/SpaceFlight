using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField, Range(0f,5f)]
    public float rotationSpeed = 5f;
    public float dampAmount = 2f;
    [SerializeField]
    public bool reverse = false;

    private void Awake()
    {
        rotationSpeed = reverse ? -rotationSpeed : rotationSpeed;
    }
    void FixedUpdate()
    {
        transform.Rotate((Vector3.up * rotationSpeed) * (Time.deltaTime * dampAmount), Space.Self);
    }
}

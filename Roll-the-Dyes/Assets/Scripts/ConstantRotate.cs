using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float degPerSecond;

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(degPerSecond * Time.deltaTime, Vector3.up);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] private GameObject focalObj = null;
    [SerializeField] private bool lookAtObj;
    private Vector3 camOffset;

    void Start()
    {
        //Ensure this object has no parents mucking up its transform, so it can move independently.
        gameObject.transform.parent = null;

        //Get the position the camera should be relative to its focal object.
        camOffset = transform.position - focalObj.transform.position;
    }

    void Update()
    {
        if (!focalObj) return;

        transform.position = focalObj.transform.position + camOffset;

        if (lookAtObj)
        {
            transform.LookAt(focalObj.transform);
        }
    }
}

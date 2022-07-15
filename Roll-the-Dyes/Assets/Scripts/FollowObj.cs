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
        if (focalObj)
        {
            transform.position = new Vector3
                (
                    focalObj.transform.position.x + camOffset.x,
                    focalObj.transform.position.y + camOffset.y,
                    focalObj.transform.position.z + camOffset.z
                );

            if (lookAtObj)
            {
                transform.LookAt(focalObj.transform);
            }
        }
    }
}

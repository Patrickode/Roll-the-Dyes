using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwivelObj : MonoBehaviour
{
    [SerializeField] private float digitalRotateSpeed = 1;
    private Vector2 digitalSwivelAxis;

    [SerializeField] private float mouseRotateSpeed = 1;
    [Tooltip("The max the mouse rotation axis can be. Exceeding values will be ignored, not clamped.")]
    [SerializeField] [Min(1.5f)] private float maxMouseDelta = 100;
    private Vector3 prevMousePos;
    private Vector3 mouseSwivelAxis;
    private Vector3 prevMSwivAxis;

    private bool usingDigital;

    private bool GetDigitalSwivelInput()
    {
        digitalSwivelAxis = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            digitalSwivelAxis.y--;
        if (Input.GetKey(KeyCode.RightArrow))
            digitalSwivelAxis.y++;

        if (Input.GetKey(KeyCode.UpArrow))
            digitalSwivelAxis.x++;
        if (Input.GetKey(KeyCode.DownArrow))
            digitalSwivelAxis.x--;

        return !(digitalSwivelAxis == Vector2.zero);
    }

    private Vector3 GetMouseSwivelInput()
    {
        mouseSwivelAxis = Vector3.zero;

        if ((Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
            || (Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
            || (Input.GetMouseButton(2) && !Input.GetMouseButtonUp(2)))
        {
            mouseSwivelAxis = Input.mousePosition - prevMousePos;
        }

        prevMousePos = Input.mousePosition;
        return mouseSwivelAxis;
    }

    void Update()
    {
        usingDigital = false;

        if (GetDigitalSwivelInput())
        {
            transform.localRotation = Quaternion.Euler(
                transform.localRotation.eulerAngles.x + digitalSwivelAxis.x * digitalRotateSpeed * Time.deltaTime,
                transform.localRotation.eulerAngles.y + digitalSwivelAxis.y * digitalRotateSpeed * Time.deltaTime,
                0);
            usingDigital = true;
        }
    }

    private void LateUpdate()
    {
        if (usingDigital) return;

        GetMouseSwivelInput();
        if (mouseSwivelAxis == Vector3.zero
            || (mouseSwivelAxis - prevMSwivAxis).sqrMagnitude > maxMouseDelta * maxMouseDelta)
        {
            prevMSwivAxis = Vector3.zero;
            return;
        }

        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x + -mouseSwivelAxis.y * mouseRotateSpeed * Time.deltaTime,
            transform.localRotation.eulerAngles.y + mouseSwivelAxis.x * mouseRotateSpeed * Time.deltaTime,
            0);

        prevMSwivAxis = mouseSwivelAxis;
    }
}

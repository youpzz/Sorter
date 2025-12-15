using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float lookXLimit = 80f;


    private float rotationX = 0;
    private bool canMove = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        HandleCameraRotation();
    }


    private void HandleCameraRotation()
    {
        if (!canMove) return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float rotationY = Input.GetAxis("Mouse X") * lookSensitivity;
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);
    }
}

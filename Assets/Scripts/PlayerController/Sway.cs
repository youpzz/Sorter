using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Settings")]
    public float swayClamp = 0.09f;

    [Space]
    public float smoothing = 3f;

    private Vector3 origin;

    void Start()
    {
        this.origin = transform.localPosition;
    }

    private void Update()
    {
        // if (PlayerMovement.Instance.CanMove == false) return;
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(input.x, -this.swayClamp, this.swayClamp);
        input.y = Mathf.Clamp(input.y, -this.swayClamp, this.swayClamp);

        Vector3 target = new Vector3(-input.x, -input.y, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * this.smoothing);


    }

}

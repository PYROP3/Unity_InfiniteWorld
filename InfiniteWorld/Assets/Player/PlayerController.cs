#define DEBUG_CC2D_RAYS
using UnityEngine;
using System;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    public Camera _camera;

	Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    void Start ()
    {
    body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    // Gives a value between -1 and 1
    horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
    vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        } 

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _camera.transform.position.z);
    }
}
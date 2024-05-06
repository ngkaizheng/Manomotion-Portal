using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 10;

    private void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check for user input to add force in the Z direction
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Add force in the local forward direction (Z direction) of the object
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{

    public Transform BluePos;
    public Transform YellowPos;

    void Update()
    {
        // Check if a GameObject with the tag "PortalYellow" exists
        GameObject yellowPortal = GameObject.FindGameObjectWithTag("YellowPos");

        // If a yellow portal is found, assign its transform to YellowPos
        if (yellowPortal != null)
        {
            YellowPos = yellowPortal.transform;
        }

        // Check if a GameObject with the tag "PortalYellow" exists
        GameObject bluePortal = GameObject.FindGameObjectWithTag("BluePos");

        // If a yellow portal is found, assign its transform to YellowPos
        if (bluePortal != null)
        {
            BluePos = bluePortal.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (gameObject.CompareTag("PortalBlue"))
        {
            Debug.Log("Enter Blue Portal");
            other.transform.position = YellowPos.transform.position;
            other.transform.rotation = YellowPos.transform.rotation;
            if (rb != null)
            {
                Debug.Log("rb.velocity before: " + rb.velocity);
                // //Convert the velocity of the object to the local space of the BluePos
                // Vector3 localVelocity = BluePos.InverseTransformDirection(rb.velocity);
                // //Asign the velocity to the object in the local space of the YellowPos
                // rb.velocity = YellowPos.TransformDirection(localVelocity);
                // Calculate the opposite direction of the local up vector of YellowPos
                // Vector3 oppositeDirection = -YellowPos.up;

                // // Calculate the rotation needed to face the opposite direction
                // Quaternion targetRotation = Quaternion.LookRotation(oppositeDirection, YellowPos.forward);

                // // Set the rotation of the object to the target rotation
                // other.transform.rotation = targetRotation;

                // // Calculate the remaining force in the direction of YellowPos's local up vector
                // Vector3 remainingForce = Vector3.Project(rb.velocity, -YellowPos.up);

                // // Apply the remaining force in the direction of YellowPos's local up vector
                // rb.velocity = remainingForce;
                rb.useGravity = true;
                rb.velocity = -rb.velocity;
                Debug.Log("rb.velocity after: " + rb.velocity);

            }
        }

        if (gameObject.CompareTag("PortalYellow"))
        {
            Debug.Log("Enter Yellow Portal");
            other.transform.position = BluePos.transform.position;
            other.transform.rotation = BluePos.transform.rotation;
            if (rb != null)
            {
                rb.useGravity = true;
                rb.velocity = -rb.velocity;
            }
        }
    }
}

// public class PortalManager : MonoBehaviour
// {
//     public Transform BluePos;
//     public Transform YellowPos;
//     public GameObject cameraYellow; // Reference to the CameraYellow GameObject
//     public GameObject cameraBlue; // Reference to the CameraYellow GameObject

//     private void OnTriggerEnter(Collider other)
//     {
//         Debug.Log("OnTriggerEnter");

//         Rigidbody rb = other.GetComponent<Rigidbody>(); // Get the Rigidbody component of the collided GameObject

//         if (rb != null) // Check if the collided object has a Rigidbody
//         {
//             Debug.Log("Enter Portal with Rigidbody");

//             if (gameObject.CompareTag("PortalBlue"))
//             {
//                 Debug.Log("Enter Blue Portal");
//                 TeleportObject(rb, YellowPos, cameraYellow);
//             }
//             else if (gameObject.CompareTag("PortalYellow"))
//             {
//                 Debug.Log("Enter Yellow Portal");
//                 TeleportObject(rb, BluePos, cameraBlue);
//             }
//         }
//     }

//     // Function to teleport the object and transfer its momentum towards the direction of the "CameraYellow" object
//     private void TeleportObject(Rigidbody rb, Transform destination, GameObject camera)
//     {
//         Vector3 directionToCamera = (camera.transform.position - destination.position).normalized; // Calculate direction to CameraYellow
//         float originalSpeed = rb.velocity.magnitude; // Get the original speed of the object
//         Vector3 newVelocity = directionToCamera * originalSpeed; // Calculate new velocity towards the CameraYellow direction
//         rb.velocity = newVelocity; // Set the new velocity to the Rigidbody
//         rb.angularVelocity = Vector3.zero; // Reset angular velocity to prevent unwanted rotation
//         rb.position = destination.position; // Teleport the object to the destination
//     }
// }
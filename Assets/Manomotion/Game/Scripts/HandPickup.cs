using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPickup : MonoBehaviour
{
    private bool isGrabbing = false;

    private bool isPickGesture = false;
    private GameObject grabbedObject = null;

    private Camera mainCamera;

    private SkeletonManager skeletonManager;

    void Start()
    {
        skeletonManager = FindObjectOfType<SkeletonManager>();
        if (skeletonManager == null)
        {
            Debug.LogError("SkeletonManager not found in the scene.");
        }
        else
        {
            Debug.Log("SkeletonManager found in the scene.");
        }

        // Assign the main camera if not explicitly set
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    private void Update()
    {
        if (ManomotionManager.Instance.Hand_infos.Length > 0)
        {
            GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

            Debug.Log("Gesture: " + gesture.mano_gesture_trigger);
            OnManoGestureTrigger(gesture.mano_gesture_trigger);
        }

        if (isPickGesture)
        {
            GameObject trackGameObject = GameObject.Find("SphereCollider(Clone)");
            // Get the position of the palm center detected by ManoMotion
            // Vector3 handPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;
            // Vector3 handPosition = skeletonManager._listOfJoints[1].transform.position;
            Vector3 handPosition = trackGameObject.transform.position;
            Debug.Log("Hand Position: " + handPosition);

            // If hand position is valid (not Vector3.zero), move the grabbed object to the hand position
            if (grabbedObject != null)
            {
                if (handPosition != Vector3.zero)
                {
                    grabbedObject.transform.position = handPosition;
                }
            }
        }
    }

    // private void pickingmovement()
    // {
    //     if (isGrabbing && grabbedObject != null && isPickGesture)
    //     {
    //         GameObject trackGameObject = GameObject.Find("SphereCollider(Clone)");
    //         // Get the position of the palm center detected by ManoMotion
    //         // Vector3 handPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;
    //         // Vector3 handPosition = skeletonManager._listOfJoints[1].transform.position;
    //         Vector3 handPosition = trackGameObject.transform.position;
    //         Debug.Log("Hand Position: " + handPosition);

    //         // If hand position is valid (not Vector3.zero), move the grabbed object to the hand position
    //         if (handPosition != Vector3.zero)
    //         {
    //             grabbedObject.transform.position = handPosition;
    //         }
    //     }
    // }

    private void OnManoGestureTrigger(ManoGestureTrigger gestureTrigger)
    {
        // pickingmovement();

        GameObject trackGameObject = GameObject.Find("SphereCollider(Clone)");
        if (gestureTrigger == ManoGestureTrigger.PICK && isGrabbing)
        {
            isPickGesture = true;

            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            Vector3 handPosition = trackGameObject.transform.position;

            // If hand position is valid (not Vector3.zero), move the grabbed object to the hand position
            if (handPosition != Vector3.zero)
            {
                grabbedObject.transform.position = handPosition;
            }

        }
        if (gestureTrigger == ManoGestureTrigger.DROP)
        {
            // Check if the hand is currently grabbing an object
            if (isGrabbing && grabbedObject != null)
            {
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                // if (rb != null)
                // {
                //     // Add a force in the Z direction
                //     rb.AddForce(Vector3.forward * 4, ForceMode.Impulse);
                // }
                if (rb != null && mainCamera != null)
                {
                    // Calculate the force direction based on the camera's forward vector
                    Vector3 forceDirection = mainCamera.transform.forward;

                    // Add a force in the direction of the camera's forward vector
                    rb.AddForce(forceDirection * 35, ForceMode.Impulse);
                }
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;

                // Reset the grabbedObject and isGrabbing flag
                grabbedObject = null;
                isGrabbing = false;
            }
            else
            {
                Debug.LogWarning("No object to release.");
            }
            isPickGesture = false;
            isGrabbing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter Hand With OtherObject");
        if (!isGrabbing && other.CompareTag("Grabable"))
        {
            // When the hand collides with the cube, "pick up" the cube
            grabbedObject = other.gameObject;
            isGrabbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit Hand With OtherObject");
        if (isGrabbing && other.gameObject == grabbedObject)
        {
            // // Release the grabbed object
            // Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     // Add a force in the Z direction
            //     rb.AddForce(Vector3.forward * 2, ForceMode.Impulse);
            // }
            // When the hand stops colliding with the cube, release the cube
            grabbedObject = null;
            isGrabbing = false;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("OnTriggerEnter Hand With OtherObject");
    //     if (!isGrabbing && other.CompareTag("Grabable"))
    //     {
    //         // When the hand collides with the grabable object, assign it to grabbedObject
    //         grabbedObject = other.gameObject;
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     Debug.Log("OnTriggerExit Hand With OtherObject");
    //     if (isGrabbing && other.gameObject == grabbedObject)
    //     {
    //         // When the hand stops colliding with the grabable object, release it
    //         grabbedObject = null;
    //     }
    // }

}

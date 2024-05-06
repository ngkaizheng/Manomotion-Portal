using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPickupcopy : MonoBehaviour
{
    private bool isGrabbing = false;
    private GameObject grabbedObject = null;

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
    }
    private void Update()
    {
        if (ManomotionManager.Instance.Hand_infos.Length > 0)
        {
            GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

            Debug.Log("Gesture: " + gesture.mano_gesture_trigger);
            OnManoGestureTrigger(gesture.mano_gesture_trigger);
        }

        // if (isGrabbing && grabbedObject != null)
        // {
        //     GameObject trackGameObject = GameObject.Find("SphereCollider(Clone)");
        //     // Get the position of the palm center detected by ManoMotion
        //     // Vector3 handPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;
        //     // Vector3 handPosition = skeletonManager._listOfJoints[1].transform.position;
        //     Vector3 handPosition = trackGameObject.transform.position;
        //     Debug.Log("Hand Position: " + handPosition);

        //     // If hand position is valid (not Vector3.zero), move the grabbed object to the hand position
        //     if (handPosition != Vector3.zero)
        //     {
        //         grabbedObject.transform.position = handPosition;
        //     }
        // }
    }

    private void OnManoGestureTrigger(ManoGestureTrigger gestureTrigger)
    {
        if (gestureTrigger == ManoGestureTrigger.PICK)
        {
            // Check if the hand is already grabbing an object
            if (!isGrabbing)
            {
                // Set the grabbing flag to true
                isGrabbing = true;

                // Check if there's a GameObject to grab
                if (grabbedObject == null)
                {
                    Debug.LogWarning("No grabbable object detected.");
                    return;
                }

                // Pick up the grabbedObject
                // You can implement your pickup logic here
                // For example, you can disable the Rigidbody of the grabbedObject to prevent it from being affected by physics
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 handPosition = skeletonManager._listOfJoints[1].transform.position;

                // If hand position is valid (not Vector3.zero), move the grabbed object to the hand position
                if (handPosition != Vector3.zero)
                {
                    grabbedObject.transform.position = handPosition;
                }
            }
            else
            {
                Debug.LogWarning("Already grabbing an object.");
            }
        }
        if (gestureTrigger == ManoGestureTrigger.DROP)
        {
            // Check if the hand is currently grabbing an object
            if (isGrabbing && grabbedObject != null)
            {
                // Release the grabbedObject
                // You can implement your release logic here
                // For example, you can enable the Rigidbody of the grabbedObject to allow it to be affected by physics again
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                // Reset the grabbedObject and isGrabbing flag
                grabbedObject = null;
                isGrabbing = false;
            }
            else
            {
                Debug.LogWarning("No object to release.");
            }
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
            // Release the grabbed object
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Add a force in the Z direction
                rb.AddForce(Vector3.forward * 2, ForceMode.Impulse);
            }
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

using UnityEngine;
using ManoMotion;

public class SpawnManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject spherePrefab;

    private GameObject grabbedObject; // Reference to the currently grabbed object

    private bool isPicking; // Flag to indicate if the grab gesture is active

    private SkeletonManager skeletonManager;

    private bool generateObject = true;

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


    void Update()
    {
        if (!generateObject)
        {
            return;
        }
        if (ManomotionManager.Instance.Hand_infos.Length > 0)
        {
            GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;

            Debug.Log("Gesture: " + gesture.mano_gesture_trigger);
            OnManoGestureTrigger(gesture.mano_gesture_trigger);

        }


        if (isPicking)
        {
            // Continuously update the position of the grabbed object to follow the palm's position
            if (grabbedObject != null)
            {
                // Get the screen space position of the palm center
                Vector3 screenPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;

                // // Convert screen space position to world space position
                // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                // Get the depth estimation for the hand
                float handDepth = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation;

                // Set the z-coordinate of the world position to the depth estimation
                screenPosition.z = handDepth * 10;

                // Update the position of the grabbed object to follow the palm's position
                grabbedObject.transform.position = screenPosition;
            }
        }


    }

    public void SetGenerateObject()
    {
        generateObject = !generateObject;
    }

    // private void OnManoGestureTrigger(ManoGestureTrigger gestureTrigger)
    // {
    //     if (gestureTrigger == ManoGestureTrigger.GRAB_GESTURE)
    //     {
    //         Vector3 clickPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;
    //         Debug.Log("Grab gesture detected at: " + clickPosition);
    //         Instantiate(cubePrefab, clickPosition, Quaternion.identity);
    //     }
    // }

    private void OnManoGestureTrigger(ManoGestureTrigger gestureTrigger)
    {
        // if (gestureTrigger == ManoGestureTrigger.GRAB_GESTURE)
        // {
        //     // Get the screen space position of the palm center
        //     // Vector3 screenPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.poi;
        //     Vector3 screenPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8];

        //     Debug.Log("Grab gesture screenPosition at: " + screenPosition);
        //     // Get the depth estimation for the hand
        //     float handDepth = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation;

        //     Debug.Log("Hand depth: " + handDepth);
        //     // Set the z-coordinate of the world position to the depth estimation
        //     screenPosition.z = handDepth * 10;

        //     // Instantiate cube prefab at the converted world space position
        //     Instantiate(cubePrefab, screenPosition, Quaternion.identity);
        // }

        if (gestureTrigger == ManoGestureTrigger.CLICK)
        {
            // Get the screen space position of the palm center
            // Vector3 screenPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.poi;
            // Vector3 screenPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8];
            Vector3 screenPosition = skeletonManager._listOfJoints[8].transform.position;

            Debug.Log("Grab gesture screenPosition at: " + screenPosition);
            // // Get the depth estimation for the hand
            // float handDepth = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation;

            // Debug.Log("Hand depth: " + handDepth);
            // // Set the z-coordinate of the world position to the depth estimation
            // screenPosition.z = handDepth * 10;

            // Instantiate cube prefab at the converted world space position 
            Instantiate(spherePrefab, screenPosition, Quaternion.identity);
            // Instantiate(cubePrefab, screenPosition, Quaternion.identity);
        }

        // if (gestureTrigger == ManoGestureTrigger.PICK)
        // {
        //     // Set grabbing flag to true
        //     isPicking = true;

        //     // Check if there's a GameObject to grab
        //     // For simplicity, you can directly instantiate a cubePrefab or another GameObject here
        //     // Or, you can have a separate mechanism to detect and set the grabbedObject
        //     grabbedObject = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);

        // }
        // if (gestureTrigger == ManoGestureTrigger.DROP)
        // {
        //     // Release the grabbed object
        //     // Destroy(grabbedObject);
        //     grabbedObject = null;

        //     // Reset grabbing flag
        //     isPicking = false;
        // }

        // if (gestureTrigger == ManoGestureTrigger.GRAB_GESTURE)
        // {
        //     // Set grabbing flag to true
        //     isPicking = true;

        //     // Check if there's a GameObject to grab
        //     // For simplicity, you can directly instantiate a cubePrefab or another GameObject here
        //     // Or, you can have a separate mechanism to detect and set the grabbedObject
        //     grabbedObject = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);

        // }
        // if (gestureTrigger == ManoGestureTrigger.RELEASE_GESTURE)
        // {
        //     // Release the grabbed object
        //     // Destroy(grabbedObject);
        //     grabbedObject = null;

        //     // Reset grabbing flag
        //     isPicking = false;
        // }

    }


}

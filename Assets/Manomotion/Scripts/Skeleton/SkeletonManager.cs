using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the visualization of the skeleton joints.
/// </summary>
public class SkeletonManager : MonoBehaviour
{

    #region Singleton
    /// <summary>
    /// Creates instance of SkeletonManager
    /// </summary>
    public static SkeletonManager instance;

    public GameObject sphereColliderPrefab; // Prefab for the sphere collider
    private GameObject sphereColliderInstance; // Instance of the sphere collider GameObject

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            this.gameObject.SetActive(false);
            Debug.LogWarning("More than 1 SkeletonManager in scene");
        }
    }
    #endregion

    [HideInInspector]
    ///The list of joints used for visualization
    public List<GameObject> _listOfJoints = new List<GameObject>();

    ///The prefab that will be used for visualization of the joints 
    [SerializeField]
    private GameObject[] jointPrefab;

    ///The linerenderes used on the joints in the jointPrefabs
    private LineRenderer[] lineRenderers = new LineRenderer[6];

    ///used to clamp the depth value
    private float clampMinDepth = 0.4f;

    ///Skeleton confidence
    private bool hasConfidence;
    private float skeletonConfidenceThreshold = 0.0001f;

    ///The materials used on the joints / Line renderers
    [SerializeField]
    private Material[] jointsMaterial;

    /// The number of Joints the skeleton is made of.
    private int jointsLength = 21;

    private SkeletonInfo skeletonInfo;

    private void Start()
    {
        Inititialize();
    }

    void Inititialize()
    {

        for (int i = 0; i < jointPrefab.Length; i++)
        {
            jointPrefab[i] = Instantiate(jointPrefab[i]);
        }

        SkeletonModel(0, 1);

        ManomotionManager.OnSkeleton3dActive += SkeletonModel;

        for (int i = 0; i < jointsMaterial.Length; i++)
        {
            Color tempColor = jointsMaterial[i].color;
            tempColor.a = 0f;
            jointsMaterial[i].color = tempColor;
        }
    }

    /// <summary>
    /// Create the hand model depending if you use 3D or 2D joints.
    /// The model need to have 21 joints.
    /// </summary>
    /// <param name="modelToLoad">The current model displayed</param>
    /// <param name="previousModel">The previous model used</param>
    private void SkeletonModel(int modelToLoad, int previousModel)
    {
        if (jointPrefab[modelToLoad].transform.childCount == jointsLength)
        {
            _listOfJoints.Clear();

            jointPrefab[previousModel].SetActive(false);
            jointPrefab[modelToLoad].SetActive(true);

            for (int i = 0; i < jointPrefab[modelToLoad].transform.childCount; i++)
            {
                _listOfJoints.Add(jointPrefab[modelToLoad].transform.GetChild(i).gameObject);
            }

            lineRenderers = new LineRenderer[6];
            lineRenderers = (jointPrefab[modelToLoad].GetComponentsInChildren<LineRenderer>());
            ResetLineRenderers();
        }

        else
        {
            Debug.LogFormat("Current model have {0} joints, need to have 21 joints", jointPrefab[modelToLoad].transform.childCount);
        }
    }

    /// <summary>
    /// Reset the Linerenders when changing Skeleton Model 2D/3D
    /// </summary>
    private void ResetLineRenderers()
    {
        foreach (var item in lineRenderers)
        {
            item.enabled = true;
            item.positionCount = 0;
            item.positionCount = 4;
        }

        lineRenderers[1].positionCount = 6;
    }


    void Update()
    {
        skeletonInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton;
        hasConfidence = skeletonInfo.confidence > skeletonConfidenceThreshold;
        UpdateJointPositions();
        UpdateJointorientation();

        // // Update the position of the sphere collider to match the hand's position
        // if (hasConfidence)
        // {
        //     // Vector3 handPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8];
        //     Vector3 handPosition = _listOfJoints[9].transform.position;

        //     // Check if the sphere collider instance exists
        //     if (sphereColliderInstance == null)
        //     {
        //         // Instantiate the sphere collider prefab
        //         sphereColliderInstance = Instantiate(sphereColliderPrefab);
        //     }

        //     // Set the position of the sphere collider instance
        //     sphereColliderInstance.transform.position = handPosition;
        // }
        // else
        // {
        //     // Destroy the sphere collider instance if confidence is too low
        //     if (sphereColliderInstance != null)
        //     {
        //         Destroy(sphereColliderInstance);
        //     }
        // }

        // Update the position of the sphere collider to match the hand's position
        if (hasConfidence)
        {
            // Vector3 handPosition = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8];
            // Vector3 handPosition = _listOfJoints[9].transform.position;
            Vector3 handPosition = (_listOfJoints[4].transform.position + _listOfJoints[8].transform.position) / 2f;

            // // Find the parent GameObject by name
            // GameObject parentGameObject = GameObject.Find("2D Joints(Clone)");

            // if (parentGameObject != null)
            // {
            //     // Check if the sphere collider instance exists
            //     if (sphereColliderInstance == null)
            //     {
            //         // Instantiate the sphere collider prefab under the parent GameObject
            //         sphereColliderInstance = Instantiate(sphereColliderPrefab, parentGameObject.transform);

            //         // Optionally, you can reset the local position and rotation of the collider
            //         sphereColliderInstance.transform.localPosition = Vector3.zero;
            //         sphereColliderInstance.transform.localRotation = Quaternion.identity;
            //     }

            //     // Set the position of the sphere collider instance
            //     sphereColliderInstance.transform.position = handPosition;
            // }
            // else
            // {
            //     Debug.LogError("Parent GameObject '2D Joints(Clone)' not found.");
            // }

            GameObject parent2D = GameObject.Find("2D Joints(Clone)");
            GameObject parent3D = GameObject.Find("3D Joints(Clone)");

            // Check if either parent GameObject exists
            if (parent2D != null || parent3D != null)
            {
                // Determine which parent GameObject to use
                GameObject parentGameObject = (parent2D != null) ? parent2D : parent3D;

                // Check if the sphere collider instance exists
                if (sphereColliderInstance == null)
                {
                    // Instantiate the sphere collider prefab under the selected parent GameObject
                    sphereColliderInstance = Instantiate(sphereColliderPrefab, parentGameObject.transform);

                    // Optionally, you can reset the local position and rotation of the collider
                    sphereColliderInstance.transform.localPosition = Vector3.zero;
                    sphereColliderInstance.transform.localRotation = Quaternion.identity;
                }

                // Set the position of the sphere collider instance
                sphereColliderInstance.transform.position = handPosition;
            }
            else
            {
                Debug.LogError("Parent GameObject '2D Joints(Clone)' or '3D Joints(Clone)' not found.");
            }
        }
        else
        {
            // Destroy the sphere collider instance if confidence is too low
            if (sphereColliderInstance != null)
            {
                Destroy(sphereColliderInstance);
            }
        }

    }

    /// <summary>
    /// Calculates the radiant value to degrees
    /// </summary>
    /// <param name="radiantValue">the radiant value</param>
    /// <returns></returns>
    private float radianToDegrees(float radiantValue)
    {
        float degreeValue;
        degreeValue = radiantValue * Mathf.Rad2Deg;
        return degreeValue;
    }

    /// <summary>
    /// Updates the orientation of the joints according to the orientation given by the SDK.
    /// </summary>
    private void UpdateJointorientation()
    {
        if (hasConfidence)
        {
            for (int i = 0; i < skeletonInfo.orientation_joints.Length; i++)
            {
                float xRotation = radianToDegrees(skeletonInfo.orientation_joints[i].x);
                float yRotation = radianToDegrees(skeletonInfo.orientation_joints[i].y);
                float zRotation = radianToDegrees(skeletonInfo.orientation_joints[i].z);

                //Correct the joint orientation if left hand is used with back facing orienations
                if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.left_right_hand == LeftOrRightHand.LEFT_HAND && (int)ManomotionManager.Instance.Manomotion_Session.orientation < 7)
                {
                    yRotation = radianToDegrees((3.14f + skeletonInfo.orientation_joints[i].y));
                }

                //Correct the joint orientation if right hand is used with front facing orienations 
                if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.left_right_hand == LeftOrRightHand.RIGHT_HAND && (int)ManomotionManager.Instance.Manomotion_Session.orientation > 6)
                {
                    yRotation = radianToDegrees((3.14f + skeletonInfo.orientation_joints[i].y));
                }

                switch (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side)
                {
                    case HandSide.None:
                        break;
                    case HandSide.Backside:
                        break;
                    case HandSide.Palmside:
                        xRotation = -xRotation;
                        zRotation = -zRotation;
                        break;
                    default:
                        break;
                }

                Vector3 newRotation = new Vector3(xRotation, yRotation, zRotation);

                _listOfJoints[i].transform.eulerAngles = newRotation;
            }
        }
    }

    /// <summary>
    /// Updates the position of the joints according to the positions given by the SDK.
    /// If confidence is to low, the joints will fade out.
    /// </summary>
    private void UpdateJointPositions()
    {
        if (hasConfidence)
        {

            if (jointsMaterial[jointsMaterial.Length - 1].color.a < 1)
            {
                for (int i = 0; i < jointsMaterial.Length; i++)
                {
                    Color tempColor = jointsMaterial[i].color;
                    tempColor.a += 0.1f;
                    jointsMaterial[i].color = tempColor;
                }
            }

            for (int i = 0; i < skeletonInfo.joints.Length; i++)
            {
                float depthEstimation = Mathf.Clamp(ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation, clampMinDepth, 1);

                Vector3 newPosition3d = ManoUtils.Instance.CalculateNewPositionSkeletonPosition(new Vector3(skeletonInfo.joints[i].x, skeletonInfo.joints[i].y, skeletonInfo.joints[i].z), depthEstimation * 10);

                _listOfJoints[i].transform.position = newPosition3d;
            }
        }

        else
        {
            if (jointsMaterial[0].color.a > 0)
            {
                for (int i = 0; i < jointsMaterial.Length; i++)
                {
                    Color tempColor = jointsMaterial[i].color;
                    tempColor.a -= 0.1f;
                    jointsMaterial[i].color = tempColor;
                }
            }
        }
    }
}
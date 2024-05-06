using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePortalManager : MonoBehaviour
{
    public GameObject[] portalPrefab;

    private GameObject[] portalPrefabInstance = new GameObject[2];

    public GameObject spherePrefab;

    public GameObject sphereContainer; // Reference to the GameObject under which spheres will be generated


    public bool isBluePortal = true;

    public bool generatePortal = true;

    private GameObject[] sphere = new GameObject[8];
    public Material greenMaterial;
    private SkeletonManager skeletonManager;
    // public Transform referenceTransform; // Reference transform for world space

    private List<Vector3> fingerPassThroughPoints = new List<Vector3>();

    // private Vector3 point1;
    // private Vector3 point2 = new Vector3(100f, 0f, 0f);

    int numPoints = 8; // Number of points around the circle
    private Vector3[] point = new Vector3[8]; // Array to store the positions of the points

    private Vector3 trackPoint;

    private Vector3 center;

    private bool[] overlap = new bool[8];

    private float updateInterval = 10f; // Update interval in seconds
    private float timer = 0f;

    private bool canGeneratePortal = true; // Flag to control portal generation cooldown


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
        UpdatePoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the rotation of the camera
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Define the desired rotation for the portal
        Quaternion portalRotation = Quaternion.Euler(0f, 90f, 90f);

        // Combine the rotations of the camera and the portal
        Quaternion finalRotation = cameraRotation * portalRotation;

        Vector3 eulerAngle = finalRotation.eulerAngles;

        Debug.Log("finalRotation: " + finalRotation);

        Debug.Log("eulerAngle: " + eulerAngle);

        if (!generatePortal)
        {
            if (GameObject.FindGameObjectsWithTag("SpawnedSphere") != null)
            {

                //Destory sphere[i]
                for (int i = 0; i < numPoints; i++)
                {
                    Destroy(sphere[i]);
                }
            }
            return;
        }
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to update point2
        if (timer >= updateInterval)
        {
            // Update point2
            UpdatePoint();

            // Reset the timer
            timer = 0f;
        }

        // Detect finger position using the index finger joint
        Vector3 fingerPosition = skeletonManager._listOfJoints[8].transform.position;

        // Store finger positions
        fingerPassThroughPoints.Add(fingerPosition);

        // Ensure the array doesn't exceed 5 elements
        if (fingerPassThroughPoints.Count > 5)
        {
            // Remove the oldest elements
            fingerPassThroughPoints.RemoveAt(0);
        }


        if (fingerPassThroughPoints.Count >= 1)
        {
            trackPoint = fingerPassThroughPoints[fingerPassThroughPoints.Count - 1];

            checkGeneratePortal();
        }

    }

    public void SetIsBluePortal()
    {
        isBluePortal = !isBluePortal;
    }

    public void SetGeneratePortal()
    {
        generatePortal = !generatePortal;
    }

    private void checkGeneratePortal()
    {
        // float distance = Vector3.Distance(point[0], point[1]);
        // float tolerance = 2f;
        // if (distance <= tolerance && canGeneratePortal)
        // {
        //     Debug.Log("YesGeneratedPortal");
        //     Instantiate(portalPrefab, point[0], Quaternion.Euler(0f, 90f, 90f));
        //     StartCoroutine(PortalCooldown());
        // }

        float tolerance = 1.25f;

        // Check distance from trackPoint to each point in the point array
        for (int i = 0; i < numPoints; i++)
        {
            float distance = Vector3.Distance(trackPoint, point[i]);
            if (distance <= tolerance)
            {
                overlap[i] = true;

                if (sphere[i] == null)
                {
                    return;
                }
                //Assign corresponding material to the sphere
                sphere[i].GetComponent<Renderer>().material = greenMaterial;

                Debug.Log("Overlap with Point " + i);
            }
        }
        // // Check if all points have overlap
        // for (int i = 0; i < numPoints; i++)
        // {
        //     Debug.Log("Overlap with Point " + i + ": " + overlap[i]);
        // }

        bool allOverlapped = true;
        // foreach (bool pointOverlapped in overlap)
        // {
        //     if (!pointOverlapped)
        //     {
        //         allOverlapped = false;
        //         break;
        //     }
        // }
        for (int i = 0; i < overlap.Length; i++)
        {
            if (!overlap[i])
            {
                Debug.Log("overlap " + i + ": " + overlap[i]);
                allOverlapped = false;
                break;
            }
        }
        // Generate portal if all points have overlap
        if (allOverlapped && canGeneratePortal)
        {
            Debug.Log("YesGeneratedPortal");
            GameObject selectedPortalPrefab = isBluePortal ? portalPrefab[0] : portalPrefab[1];

            if (selectedPortalPrefab != null)
            {
                // // Destroy existing portal prefabs if they exist
                // for (int i = 0; i < portalPrefabInstance.Length; i++)
                // {
                //     if (portalPrefabInstance[i] != null)
                //     {
                //         Destroy(portalPrefabInstance[i]);
                //     }
                // }

                // Destroy existing portal prefab if it exists and matches the selected portal prefab
                if (portalPrefabInstance[0] != null && selectedPortalPrefab == portalPrefab[0])
                {
                    Destroy(portalPrefabInstance[0]);
                }
                // Destroy existing portal prefab if it exists and matches the selected portal prefab
                if (portalPrefabInstance[1] != null && selectedPortalPrefab == portalPrefab[1])
                {
                    Destroy(portalPrefabInstance[1]);
                }
                // Get the rotation of the camera
                Quaternion cameraRotation = Camera.main.transform.rotation;

                // Define the desired rotation for the portal
                Quaternion portalRotation = Quaternion.Euler(0f, 90f, 90f);

                // Combine the rotations of the camera and the portal
                Quaternion finalRotation = cameraRotation * portalRotation;


                // Instantiate the selected portal prefab
                portalPrefabInstance[isBluePortal ? 0 : 1] = Instantiate(selectedPortalPrefab, center, finalRotation);

                // Reset all overlap to false
                for (int i = 0; i < numPoints; i++)
                {
                    overlap[i] = false;
                }

                StartCoroutine(PortalCooldown());
            }
        }

    }

    // Update point2 based on the current finger position
    private void UpdatePoint()
    {
        //Reset all overlap to false
        for (int i = 0; i < numPoints; i++)
        {
            overlap[i] = false;
        }

        // Destroy all previously spawned spheres
        if (GameObject.FindGameObjectsWithTag("SpawnedSphere") != null)
        {

            foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("SpawnedSphere"))
            {
                Destroy(sphere);
            }
        }

        float radius = 2.0f; // Radius of the circle
        center = skeletonManager._listOfJoints[8].transform.position - (transform.up * radius); // Central point of the circle

        sphereContainer.transform.position = center; // Set the container position to the center of the circle
        sphereContainer.transform.rotation = Quaternion.identity; // Reset the container rotation

        Debug.Log("Center: " + center);
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * 360f / numPoints; // Calculate the angle for each point
            float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad); // Calculate x coordinate
            float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad); // Calculate y coordinate
            point[i] = new Vector3(x, y, center.z); // Create the point and add it to the array
            Debug.Log("Point " + i + ": " + point[i]);

            // // Instantiate the sphere prefab at the calculated point position
            // sphere[i] = Instantiate(spherePrefab, point[i], Quaternion.identity);
            // Instantiate the sphere prefab at the calculated point position relative to the container
            sphere[i] = Instantiate(spherePrefab, sphereContainer.transform);
            sphere[i].transform.position = point[i];


        }
        sphereContainer.transform.rotation = Camera.main.transform.rotation;

        //update each point[i] to sphere[i].transform.position
        for (int i = 0; i < numPoints; i++)
        {
            point[i] = sphere[i].transform.position;
        }
    }

    // Coroutine to handle portal generation cooldown
    private IEnumerator PortalCooldown()
    {
        canGeneratePortal = false;
        yield return new WaitForSeconds(5f); // Cooldown duration
        canGeneratePortal = true;
    }
}
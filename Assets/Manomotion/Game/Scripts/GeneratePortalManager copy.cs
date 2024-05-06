using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePortalManagercopy : MonoBehaviour
{
    public GameObject portalPrefab;
    private SkeletonManager skeletonManager;
    // public Transform referenceTransform; // Reference transform for world space

    private List<Vector3> fingerPassThroughPoints = new List<Vector3>();

    private Vector3 point1;
    private Vector3 point2 = new Vector3(100f, 0f, 0f);

    private Vector3[] point;


    private float updateInterval = 2f; // Update interval in seconds
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
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to update point2
        if (timer >= updateInterval)
        {
            // Update point2
            UpdatePoint2();

            // Reset the timer
            timer = 0f;
        }

        // Detect finger position using the index finger joint
        Vector3 fingerPosition = skeletonManager._listOfJoints[8].transform.position;

        // Store finger positions
        fingerPassThroughPoints.Add(fingerPosition);

        if (fingerPassThroughPoints.Count >= 1)
        {
            point1 = fingerPassThroughPoints[fingerPassThroughPoints.Count - 1];
            float distance = Vector3.Distance(point1, point2);
            float tolerance = 2f; // Adjust the tolerance as needed

            Debug.Log("Distance: " + distance);
            // Check if the distance between point1 and point2 is within the tolerance
            if (distance <= tolerance && canGeneratePortal)
            {
                Debug.Log("YesGeneratedPortal");
                // Generate portal at finger position
                Instantiate(portalPrefab, fingerPosition, Quaternion.Euler(0f, 90f, 90f));
                // Disable portal generation until cooldown expires
                StartCoroutine(PortalCooldown());

            }
        }

    }

    // Update point2 based on the current finger position
    private void UpdatePoint2()
    {
        // Calculate point2 based on the fingerPosition and the local right axis (x-axis)
        point2 = skeletonManager._listOfJoints[8].transform.position + transform.right * 5f;
        Debug.Log("Point1 updated: " + point1);
        Debug.Log("Point2 updated: " + point2);
    }

    // Coroutine to handle portal generation cooldown
    private IEnumerator PortalCooldown()
    {
        canGeneratePortal = false;
        yield return new WaitForSeconds(2f); // Cooldown duration
        canGeneratePortal = true;
    }
}